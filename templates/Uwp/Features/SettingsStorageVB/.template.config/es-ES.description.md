La configuración del almacenamiento es una clase para simplificar el almacenamiento de los datos de la aplicación.  Se ocupa de cargar, guardar, serializar tus datos y acceder fácilmente a los datos de tu aplicación.

Estos son los tipos principales de datos de la aplicación:

* Local: almacenada en el dispositivo, con copia de seguridad en la nube y persistencias en las actualizaciones
* LocalCache: datos persistentes que existen en el dispositivo actual, sin copia de seguridad y persistencias en las actualizaciones
* SharedLocal: persistencias en todos los usuarios de la aplicación
* Itinerancia: existe en todos los dispositivos en los que el usuario haya instalado la aplicación
* Temporal: puede ser eliminada por el sistema en cualquier momento

Para obtener más información acerca del almacenamiento, dirígete a [docs.microsoft.com](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata).
