Permita que várias instâncias do aplicativo sejam iniciadas e controle se novas instâncias são iniciadas ou instâncias existentes são reutilizadas. Isso inclui código de espaço reservado para permitir que você controle se reutiliza uma instância existente ou abre uma nova quando o aplicativo é iniciado.

Esse recurso só terá efeito se seu aplicativo estiver sendo executado em um dispositivo de Área de Trabalho ou IoT (Internet das Coisas).

Observe que a [documentação oficial](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/multi-instance-uwp) tem notas específicas que você deve conhecer caso esteja usando [tarefas em segundo plano](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/multi-instance-uwp#background-tasks-and-multi-instancing), bem como [considerações gerais](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/multi-instance-uwp#additional-considerations).
