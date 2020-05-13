Esta es una versión genérica de un MVVM pattern.  El [patrón Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) se puede usar en todas las plataformas XAML. Su intención es separar de forma precisa los problemas de los controles de la interfaz de usuario de su lógica.

Hay tres componentes principales en el MVVM pattern: el modelo, la vista y el modelo de vista. Cada uno tiene un rol diferente e independiente.

MVVM Basic no es un framework, pero proporciona la funcionalidad mínima para crear una aplicación usando el patrón Model-View-ViewModel (MVVM).
Úsalo si no puedes o no quieres usar un Framework MVVM de terceros.

MVVM Basic no está diseñado para ser un Framework MVVM con todas las características y no incluye algunas características que otros frameworks sí incluyen. Navegación ViewModel-first, IOC y mensajería son las más obvias. Si quieres estas características, elige un framework que sea compatible con ellas.

Los proyectos creados con MVVM Basic contienen dos clases importantes: `Observable` and `RelayCommand`.
**Observable** incluye una implementación de la interfaz `INotifyPropertyChanged` y se usa como una clase base para todos los modelos de vista. Esto facilita la actualización de las propiedades enlazadas en la Vista.
**RelayCommand** incluye una implementación de la interfaz `ICommand` para que sea fácil tener los comandos de llamada de Vista en el ViewModel y no tener que administrar los eventos de UI directamente.
