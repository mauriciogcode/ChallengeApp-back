Challenge APP - Documentación
Este documento proporciona una guía sobre la instalación, configuración y posibles mejoras para la Challenge APP.

Instalación e Implementación de Ambiente

Para instalar y configurar el entorno de desarrollo de Challenge APP, siga estos pasos:

1. Clonación del Repositorio:
Clone el repositorio desde la plataforma de alojamiento utilizando su herramienta de gestión de versiones preferida.

2. Configuración del Entorno:
Asegúrese de configurar adecuadamente las variables de entorno necesarias, como las credenciales de la base de datos u otras configuraciones específicas del proyecto.


3. Instalación de Dependencias del Frontend:
Después de clonar el repositorio, navegue hasta el directorio del proyecto y ejecute el siguiente comando para instalar las dependencias del frontend (React):


npm install


4. Instalacion de Dependencias .NET:
Instalar los paquetes nuggets requeridos. Y compilar


5. La base de datos usada se denomino Challenge. La base y la tabla usada puede ser generada por el siguiente script:

CREATE DATABASE `challenge`

ALTER DATABASE `challenge`
DEFAULT CHARACTER SET utf8mb4
DEFAULT COLLATE utf8mb4_general_ci;


CREATE TABLE bulkuploadtable (
  UserId INT AUTO_INCREMENT PRIMARY KEY,
  UserName VARCHAR(255),
  Age INT,
  Team VARCHAR(50),
  State VARCHAR(20),
  Education VARCHAR(20)
);


6. Ejecución del Proyecto:
Una vez completada la instalación de las dependencias y configuración del entorno, puede ejecutar el proyecto front utilizando el siguiente comando:

npm start

En .net se puede usar cli o visual studio. Al ejecutar la app del back se desplegara Swagger para probar la api.


7. Resolución de Problemas Comunes:

- En caso de errores durante la instalación de las dependencias npm, puede intentar utilizar el siguiente comando para resolver problemas relacionados con dependencias obsoletas:
css
- La aplicacion no recibe archivos repetidos o ya cargados previamente. Realiza una copia de los mismos en UploadFileFolder en directorio de .net para verificarlos.

npm install --legacy-peers-deps

- Si durante la subida de archivos el navegador se cierra por alguna causa externa es posible que sea necesario liberar el cache o probar con otro navegador.


9. Recomendaciones y Mejoras Futuras

A. Optimización del Procesamiento de Datos
- Implementar procesamiento por lotes para mejorar el rendimiento, especialmente en la lectura de grandes volúmenes de datos.
- Explorar formas de mejorar el rendimiento de Excel al procesar grandes cantidades de datos.

B. Mejoras en la Codificación y Gestión de Datos
- Asegurar la correcta codificación de caracteres para evitar problemas con acentos u otros caracteres especiales. Se trato de implementar UTF-8b4 general ci. Pero si el archivo esta en UTF8 es posible que los caracteres sigan fallando.


C. Mejoras en el Frontend
- Agregar confirmaciones para operaciones críticas, como la eliminación de datos.
- Refactorizar la lógica del frontend utilizando hooks y desacoplando métodos.
- Implementar mas validaciones y mejorar la consistencia del condigo con TS.

D. Mejoras en el Backend
- Desacoplar métodos en capas más específicas, como la implementación de una capa de repositorios para mejorar la mantenibilidad y la escalabilidad.
- Evaluar la implementación de patrones como Unit of Work y CQRS, sobre todo si son archivos grandes. 

E. Mejoras en la Experiencia del Usuario
- Implementar selectores dinámicos para mejorar la usabilidad de los componentes de selección.
- Proporcionar mensajes de error más claros y descriptivos para facilitar la identificación y resolución de problemas.
