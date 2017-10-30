L'impostazione dell'archiviazione è una classe per semplificare l'archiviazione dei dati applicazione.  Gestisce carico, salvataggio, serializzazione dei dati e facile accesso ai dati applicazione.

Di seguito sono indicati i principali tipi di dati dell'app:

* Local: sono archiviati sul dispositivo, ne viene eseguito il backup nel cloud e vengono mantenuti tra gli aggiornamenti
* LocalCache: sono dati persistenti presenti nel dispositivo corrente, non ne viene eseguito il backup e vengono mantenuti tra gli aggiornamenti
* SharedLocal: sono dati persistenti tra tutti gli utenti dell'app
* Roaming: sono presenti su tutti i dispositivi in cui l'utente ha installato l'app
* Temporary: possono essere eliminati dal sistema in qualsiasi momento

Per ulteriori informazioni sull'archiviazione, visita [docs.microsoft.com](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata).
