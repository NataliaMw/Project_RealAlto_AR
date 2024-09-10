# Proyecto RealAlto AR

Este proyecto es una aplicación en realidad aumentada (AR) para dispositivos móviles y tablets, diseñada para presentar objetos históricos en un entorno interactivo y educativo. A continuación se explican los aspectos principales del flujo del proyecto, los requerimientos de modelos y las características técnicas necesarias para su ejecución.

## 1. Flujo de las Scenes

El proyecto sigue un flujo de scenes que permite al usuario navegar desde una pantalla de bienvenida hasta interactuar con objetos en un entorno AR.

### 1.1 Welcome Screen
Es la pantalla inicial que el usuario ve al abrir la aplicación. Contiene el logo y la introducción del proyecto, con la opción de continuar al menú principal.

### 1.2 Menu Screen
Desde el menú principal, el usuario puede navegar a las diferentes funcionalidades de la aplicación, como iniciar el modo AR o consultar información adicional.

### 1.3 Loading Screen
Pantalla de carga que aparece mientras se preparan los recursos de la aplicación (modelos 3D, scripts, etc.). Permite una transición fluida entre el menú y el modo AR.

### 1.4 AR Mode Screen
La escena principal donde el usuario puede visualizar e interactuar con los objetos históricos en realidad aumentada, utilizando la cámara de su dispositivo.

### 1.5 Mask Screen
En esta escena, el usuario puede aplicar máscaras u otros filtros relacionados con los objetos históricos que está visualizando, ofreciendo una experiencia más interactiva.

## 2. Requerimientos de Modelos

### 2.1 Formato de los Modelos
Los modelos 3D que se asignen en la aplicación deben estar en formato `.fbx`, el cual es compatible con Unity y la mayoría de las plataformas de AR. Asegúrate de que los modelos estén optimizados para un buen rendimiento en dispositivos móviles.

### 2.2 Modelos Interactuables
Los modelos con los que el usuario puede interactuar deben incluir ciertos elementos adicionales para una experiencia completa. Estos elementos son:
- **Audio**: Un archivo de audio que proporciona información o una narración sobre el objeto.
- **Imagen**: Una imagen de referencia o información visual del objeto.
- **Título**: El nombre del objeto histórico.
- **Descripción**: Información detallada sobre el objeto, que se mostrará cuando el usuario interactúe con él.

## 3. Scripts para Objetos Interactuables

### 3.1 Asignación de Scripts a los Modelos

Cada objeto interactuable debe tener asignado un script específico que gestione su comportamiento en la aplicación. Los elementos principales del script son:

- **ObjectData**: Un componente para gestionar la información (audio, imagen, título, descripción) que se mostrará al usuario.
- **Collider**: Para detectar la interacción del usuario con el objeto en el modo AR.

#### Instrucciones para Asignar el Script:
1. Selecciona el modelo en el editor de Unity.
2. En la ventana de inspección, haz clic en `Add Component` y busca el script `ObjectData`.
3. Asigna los valores necesarios (audio, imagen, título, descripción) a los campos correspondientes del script.
4. Asegúrate de que el modelo tiene un `Collider` adecuado para detectar las interacciones del usuario.

A continuación, se muestra una imagen de referencia para configurar el script en Unity:

![Instrucciones para agregar script](ruta-de-la-imagen)

## 4. Requisitos Mínimos de Dispositivos

Para garantizar un rendimiento adecuado, la aplicación requiere que los dispositivos móviles o tablets cumplan con las siguientes características mínimas:

# Android:
- **Sistema Operativo**: Sistema operativo Android 9 o superior..
- **Resolucion Preferia**: 720 x 1280 píxeles.
- **Procesador**: Octa-core 1.8 GHz o superior.
- **RAM**: 3 GB o más.
- **Almacenamiento**: Al menos 500 MB de espacio libre.
- **Cámara**: Resolución mínima de 8 MP para una mejor experiencia en realidad aumentada.
- **Compatibilidad con ARCore o ARKit**: Es necesario que el dispositivo soporte las tecnologías AR de Google o Apple para una experiencia óptima.


---

Este documento es una guía para la correcta configuración y uso de los modelos y scripts dentro del proyecto RealAlto AR. Asegúrate de seguir estas pautas para asegurar una implementación exitosa.
