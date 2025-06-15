import socket
import threading
import json
import mysql.connector
from datetime import datetime
from dotenv import load_dotenv
import os

# Cargar variables de entorno
load_dotenv()

# Configuración del servidor
HOST = '0.0.0.0'  # Escucha en todas las interfaces
PORT = 2000  # Puerto fijo

# Configuración de la base de datos
DB_CONFIG = {
    'host': os.getenv('DB_HOST', 'localhost'),
    'user': os.getenv('DB_USER', 'root'),
    'password': os.getenv('DB_PASSWORD', ''),
    'database': os.getenv('DB_DATABASE', 'filtringapp')
}

class ChatServer:
    def __init__(self):
        self.clientes = {}  # {client_socket: {'user_id': id, 'username': name}}
        self.servidor = None
        self.db = None
        self.setup_database()

    def setup_database(self):
        """Configura la conexión a la base de datos"""
        try:
            self.db = mysql.connector.connect(**DB_CONFIG)
            print("✅ Conexión a la base de datos establecida")
        except Exception as e:
            print(f"❌ Error al conectar a la base de datos: {e}")
            exit(1)

    def verificar_match(self, id_usuario1, id_usuario2):
        """Verifica si existe un match entre dos usuarios"""
        try:
            cursor = self.db.cursor()
            query = """
                SELECT ID_Match FROM Matches 
                WHERE ((ID_Sol = %s AND ID_Acept = %s) OR (ID_Sol = %s AND ID_Acept = %s))
                AND Fecha_Aceptado IS NOT NULL
            """
            cursor.execute(query, (id_usuario1, id_usuario2, id_usuario2, id_usuario1))
            result = cursor.fetchone()
            cursor.close()
            return result is not None
        except Exception as e:
            print(f"Error al verificar match: {e}")
            return False

    def guardar_mensaje(self, id_emisor, id_receptor, contenido):
        """Guarda el mensaje en la base de datos"""
        try:
            cursor = self.db.cursor()
            # Obtener el ID_Match
            query_match = """
                SELECT ID_Match FROM Matches 
                WHERE (ID_Sol = %s AND ID_Acept = %s) OR (ID_Sol = %s AND ID_Acept = %s)
            """
            cursor.execute(query_match, (id_emisor, id_receptor, id_receptor, id_emisor))
            match_result = cursor.fetchone()
            
            if match_result:
                id_match = match_result[0]
                # Insertar el mensaje
                query = """
                    INSERT INTO Mensaje (ID_Match, ID_Emisor, ID_Receptor, Contenido, Fecha_Hora)
                    VALUES (%s, %s, %s, %s, NOW())
                """
                cursor.execute(query, (id_match, id_emisor, id_receptor, contenido))
                self.db.commit()
            cursor.close()
        except Exception as e:
            print(f"Error al guardar mensaje: {e}")

    def manejar_cliente(self, cliente_socket, direccion):
        """Maneja la comunicación con un cliente"""
        try:
            # Recibir datos de autenticación
            auth_data = cliente_socket.recv(1024).decode('utf-8')
            auth_info = json.loads(auth_data)
            user_id = auth_info['user_id']
            username = auth_info['username']

            # Almacenar información del cliente
            self.clientes[cliente_socket] = {
                'user_id': user_id,
                'username': username
            }

            print(f"[+] Usuario {username} (ID: {user_id}) conectado desde {direccion}")

            # Bucle principal de mensajes
            while True:
                mensaje_raw = cliente_socket.recv(1024).decode('utf-8')
                if not mensaje_raw:
                    break

                mensaje_data = json.loads(mensaje_raw)
                id_receptor = mensaje_data['receptor_id']
                contenido = mensaje_data['contenido']

                # Verificar match antes de enviar el mensaje
                if self.verificar_match(user_id, id_receptor):
                    # Guardar mensaje en la base de datos
                    self.guardar_mensaje(user_id, id_receptor, contenido)

                    # Preparar mensaje para enviar
                    mensaje_enviar = json.dumps({
                        'emisor_id': user_id,
                        'emisor_nombre': username,
                        'contenido': contenido,
                        'fecha': datetime.now().strftime("%Y-%m-%d %H:%M:%S")
                    })

                    # Enviar mensaje al receptor si está conectado
                    self.enviar_mensaje_a_usuario(id_receptor, mensaje_enviar)

        except Exception as e:
            print(f"Error en la conexión con {direccion}: {e}")
        finally:
            if cliente_socket in self.clientes:
                del self.clientes[cliente_socket]
            cliente_socket.close()
            print(f"[-] Cliente {direccion} desconectado")

    def enviar_mensaje_a_usuario(self, id_usuario, mensaje):
        """Envía un mensaje a un usuario específico si está conectado"""
        for cliente, info in self.clientes.items():
            if info['user_id'] == id_usuario:
                try:
                    cliente.send(mensaje.encode('utf-8'))
                except:
                    pass

    def iniciar(self):
        """Inicia el servidor"""
        self.servidor = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.servidor.bind((HOST, PORT))
        self.servidor.listen()

        print(f"🟢 Servidor de chat escuchando en {HOST}:{PORT}")

        while True:
            cliente_socket, direccion = self.servidor.accept()
            thread = threading.Thread(
                target=self.manejar_cliente,
                args=(cliente_socket, direccion)
            )
            thread.start()

if __name__ == "__main__":
    servidor = ChatServer()
    servidor.iniciar() 