# Proyecto RealAlto AR

Este proyecto es una aplicación en realidad aumentada (AR) para dispositivos móviles y tablets, diseñada para presentar objetos históricos en un entorno interactivo y educativo. A continuación se explican los aspectos principales del flujo del proyecto, los requerimientos de modelos y las características técnicas necesarias para su ejecución.

## 1. Carpetas en el Proyecto
Dento de la carpeta `Assets` se ecuenta la siguientes carpteas que forman parte de lo que se necesita para el proyecto.

- **Resources**: Carpteta donde estan todos los recursos para ser usados.
- **Scenes**: Carpeta que contiene las `scene` que pertenecen al proyecto.
- **Scripts**: Carpeta con los Scripts (codigos) que se usan en el proyecto.
  

## 2. Flujo de las Scenes

El proyecto sigue un flujo de scenes que permite al usuario navegar desde una pantalla de bienvenida hasta interactuar con objetos en un entorno AR.

### 2.1 Welcome Screen
Es la pantalla inicial que el usuario ve al abrir la aplicación. Contiene el logo, con la opción de continuar automaticamente al menú principal.

- **Transición**: Va a **Menu Screen**.

### 2.2 Menu Screen
Desde el menú principal, el usuario puede navegar a las diferentes funcionalidades de la aplicación, como iniciar el modo AR, aplicar máscaras. También tiene la opción de cerrar la aplicación.

- **Transiciones**:
  - Va a **Mask Screen**.
  - Va a **AR Mode Screen**.
  - Va a **Info Screen**. (solo una vez por sesion)
  - Puede cerrar la aplicación.

### 2.3 Mask Screen
En esta escena, el usuario puede aplicar máscaras/filtros relacionados con la cultura valdivia u otras culturas históricas que está visualizando, ofreciendo una experiencia más interactiva.

- **Transición**: Regresa a **Menu Screen**.

### 2.4 AR Mode Screen
La escena principal donde el usuario puede visualizar e interactuar con los objetos históricos en realidad aumentada, utilizando la cámara de su dispositivo y la pantalla.

- **Transición**: Regresa a **Menu Screen**.

### 2.5 Info Screen
En esta escena, el usuario presenta información adicional sobre la dimamica del recorrido AR.

- **Transición**: Va a **AR Mode Screen**.

## 3. Requerimientos de Modelos

### 3.1 Formato de los Modelos
Los modelos 3D que se asignen en la aplicación deben estar en formato `.fbx`, el cual es compatible con Unity y la mayoría de las plataformas de AR. Asegúrate de que los modelos estén optimizados para un buen rendimiento en dispositivos móviles.

### 3.2 Modelos Interactuables
Los modelos con los que el usuario puede interactuar deben incluir ciertos elementos adicionales para una experiencia completa. Estos elementos son:
- **Audio**: Un archivo de audio que proporciona información o una narración sobre el objeto.
- **Imagen**: Una imagen de referencia o información visual del objeto.
- **Título**: El nombre del objeto histórico.
- **Descripción**: Información detallada sobre el objeto, que se mostrará cuando el usuario interactúe con él.

## 4. Scripts para Objetos Interactuables

### 4.1 Asignación de Elementos a los Modelos

Cada objeto interactuable debe tener asignado algunos scripts específicos que gestione su comportamiento en la aplicación. Los elementos son:

- **Collider**: Para detectar la interacción del usuario con el objeto en el modo AR. (preferencia Box Collider)
- **ObjectData**: Un Script para gestionar la información (audio, imagen, título, descripción) que se mostrará al usuario.  
- **ProximityInfoDisplay**: Un Script para gestionar la interacción del usuario con el objeto en el modo AR. 

#### 4.2 Instrucciones para configuracion de los modelos interactuables:
1. Selecciona el modelo en el editor de Unity.
2. Asegúrate de que el modelo tiene un `Collider` adecuado para detectar las interacciones del usuario.
3. En la ventana de inspección, haz clic en `Add Component` y busca el script `ObjectData` &`ProximityInfoDisplay` o ir a la carpeta Scripts y asignarlos directamente.
4. Asigna los valores necesarios (audio, imagen, título, descripción) a los campos correspondientes del script `ObjectData`.

A continuación, se muestra una imagen de referencia para configurar el script en Unity:

## 5. Configuracion para las Mascaras
Para poder asignar mas mascaras se debe cumplir los siguientes requesitos.
- En el Inspector en Unity se debe crear un objeto tipo `Empty`
- En el `GameObject`creado hacer clic en `Add Component` y busca el script `AR Face` para asignarlo.
- En `Transform` resetear los valores de `position` o ponerlos en cero manualmente.
- Insertar la mascara como hijo del GameObject (se vuelve un prefab).
- Guardar el prefabs en la carpeta Prefabs en `Resources`.

## 6. Requisitos Mínimos de Dispositivos

Para garantizar un rendimiento adecuado, la aplicación requiere que los dispositivos móviles o tablets cumplan con las siguientes características mínimas:

### Android:
- **Sistema Operativo**: Sistema operativo Android 9 o superior.
- **Resolución Preferida**: 720 x 1280 píxeles.
- **Procesador**: Octa-core 1.8 GHz o superior.
- **RAM**: 3 GB o más.
- **Almacenamiento**: Al menos 500 MB de espacio libre.
- **Cámara**: Resolución mínima de 8 MP para una mejor experiencia en realidad aumentada.
- **Compatibilidad con ARCore o ARKit**: Es necesario que el dispositivo soporte las tecnologías AR de Google.

---

Este documento es una guía para la correcta configuración y uso de los modelos y scripts dentro del proyecto RealAlto AR. Asegúrate de seguir estas pautas para asegurar una implementación exitosa.
