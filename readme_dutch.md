# Introductie

De Quantum Katas zijn een reeks zelfstudies met eigen tempo die erop gericht zijn u tegelijkertijd elementen van quantumcomputing en Q # -programmering bij te brengen.

Elke kata behandelt een onderwerp.
Momenteel behandelde onderwerpen zijn:

* **[Basic quantum computing gates](./BasicGates/)**.
  Deze kata richt zich op de belangrijkste single-qubit en multi-qubit poorten die worden gebruikt in quantum computing.
* **[Superpositie](./Superpositie/)**.
  De taken zijn gericht op het voorbereiden van een bepaalde superpositie op een of meerdere qubits.
* **[Metingen](./Metingen/)**.
  De taken zijn gericht op het onderscheiden van kwantumtoestanden met behulp van metingen.
* **[Teleportatie](./Teleportatie/)**.
  Deze kata leidt je door het standaard teleportatieprotocol en verschillende variaties.
* **[Superdense codering](./SuperdenseCoding/)**.
  Deze kata leidt je door het superdense coderingsprotocol.
* **[Deutsch-Jozsa-algoritme](./DeutschJozsaAlgorithm/)**.
  Deze kata begint met het schrijven van quantum orakels die klassieke functies implementeren, en blijft de Bernstein-Vazirani en Deutsch-Jozsa algoritmen introduceren.
* **[Simon's algoritme](./SimonsAlgorithm/)**.
  Deze kata introduceert het algoritme van Simon.
* **[Bit-flip foutcorrectiecode](./QEC_BitFlipCode/)**.
  Deze kata introduceert een 3-qubit foutcorrigerende code voor bescherming tegen bit-flip fouten.

Elke kata is een apart project met:

* Een reeks taken over het onderwerp verloopt van triviaal naar uitdagend.
  Voor elke taak moet je een code invullen; de eerste taak kan slechts één regel vereisen, en de laatste kan een aanzienlijk fragment van code vereisen.
* Een testraamwerk dat uw oplossingen opzet, uitvoert en valideert.
  Elke taak wordt gedekt door een [* unit test *] (https://docs.microsoft.com/en-us/visualstudio/test/getting-started-with-unit-testing) die aanvankelijk faalt; zodra de test slaagt, kunt u doorgaan naar de volgende taak!
* Verwijzingen naar referentiematerialen die u mogelijk nodig hebt om de taken op te lossen, zowel op quantum computing als op Q #.
* Referentieoplossingen, voor als al het andere faalt.

# Installeren en aan de slag #

Om aan de slag te gaan met de Quantum Katas, moet u eerst de [Quantum Development Kit] (https://docs.microsoft.com/quantum), beschikbaar voor Windows 10, macOS en voor Linux, installeren.
Raadpleeg de [installatiegids voor de Quantum Development Kit] (https://docs.microsoft.com/en-us/quantum/quantum-installconfig) als u de Quantum Development Kit nog niet hebt geïnstalleerd.

Een snel naslagwerk voor Q # programmeertaal is beschikbaar [hier] (./ quickref / qsharp-quick-reference.pdf).

### De Quantum Katas downloaden ###

Als je Git hebt geïnstalleerd, doorloop je de Microsoft / QuantumKatas-repository.
Vanaf je favoriete opdrachtregel:

`` `Bash
$ git clone https://github.com/Microsoft/QuantumKatas.git
`` `

> ** TIP **: Visual Studio 2017 en Visual Studio Code maken het eenvoudig om repository's vanuit uw ontwikkelomgeving te klonen.
> Zie de [Visual Studio 2017] (https://docs.microsoft.com/en-us/vsts/git/tutorial/clone?view=vsts&tabs=visual-studio#clone-from-another-git-provider) en [Visual Studio Code] (https://code.visualstudio.com/docs/editor/versioncontrol#_cloning-a-repository) documentatie voor meer informatie.

Als alternatief, als je Git niet hebt geïnstalleerd, kun je handmatig een zelfstandig exemplaar van de katas downloaden van https://github.com/Microsoft/QuantumKatas/archive/master.zip.

### Een zelfstudie openen ###

Elke individuele kata wordt in zijn eigen directory geplaatst als een op zichzelf staande Q # -oplossing en projectpaar.
De ** BasicGates ** kata is bijvoorbeeld ingedeeld zoals hieronder.

```
QuantumKatas/
  BasicGates/
    README.md                  # Instructies specifiek voor deze kata.
    .vscode/                   # Metadata gebruikt door Visual Studio Code.
    BasicGates.sln             # Visual Studio 2017-oplossingsbestand.
    BasicGates.csproj          # Projectbestand dat wordt gebruikt om zowel klassieke als kwantumcode te bouwen.

    Tasks.qs                   # Q# broncode die u wilt invullen terwijl u elke taak oplost.
    Tests.qs                   # Q# -tests die uw oplossingen verifiëren.
    TestSuiteRunner.cs         # C# broncode gebruikt om de Q # -tests uit te voeren.
    ReferenceImplementation.qs # Q# broncode met oplossingen voor de taken.
```

Als u de ** BasicGates ** kata in Visual Studio 2017 wilt openen, opent u het oplossingsbestand `QuantumKatas / BasicGates.sln '.

Open de map `QuantumKatas / BasicGates /` om de ** BasicGates ** kata in Visual Studio Code te openen.
Druk op Ctrl + Shift + P (of ⌘ + Shift + P op macOS) om het opdrachtpalet te openen. Typ "Open Folder" op Windows 10 of Linux of "Open" op macOS.

> ** TIP **: bijna alle opdrachten die beschikbaar zijn in Visual Studio-code zijn te vinden in het opdrachtenpalet.
> Als u vastloopt, drukt u op Ctrl + Shift + P (of ⌘ + Shift + P op macOS) en typt u enkele letters om alle beschikbare opdrachten te doorzoeken.

> ** TIP **: U kunt Visual Studio-code ook starten vanaf de opdrachtregel als u daar de voorkeur aan geeft:
> `` `bash
> $ code QuantumKatas / BasicGates /
> `` `

### Kata-tests uitvoeren ###

Zodra je een kata open hebt, is het tijd om de testen uit te voeren met behulp van de onderstaande instructies.
Aanvankelijk zullen alle tests mislukken; raak niet in paniek!
Open het `Tasks.qs` bestand en begin met het invullen van de code om de taken te voltooien. Elke taak wordt gedekt door een eenheidscontrole; Zodra u de juiste code voor een taak hebt ingevuld, moet u het project opnieuw opbouwen en de tests opnieuw uitvoeren en de bijbehorende unit-test passeren.

#### Visual Studio 2017

1. Bouw een oplossing.
2. Open Test Explorer (te vinden in het menu `Test`>` Windows`) en selecteer 'Alles uitvoeren' om alle unittests tegelijk uit te voeren.
3. Werk aan de taken in het bestand `Tasks.qs`.
4. Om uw codewijzigingen voor een taak te testen, de oplossing opnieuw te bouwen en alle eenheidstests uit te voeren met "Alles uitvoeren" of de eenheidstest die die taak afdekt door met de rechtermuisknop op die test te klikken en "Geselecteerde tests uitvoeren" te selecteren.

#### Visual Studio Code

1. Druk op Ctrl + \ `(of ⌘ + \` op macOS) om de geïntegreerde terminal te openen.
   De terminal zou al in de kata-directory moeten starten, maar als dat niet het geval is, gebruik dan `cd` om naar de map met het bestand` * .csproj` voor de kata te navigeren.
2. Voer `dotnet-test` uit in de geïntegreerde terminal.
   Dit zou het kata-project automatisch moeten bouwen en alle unit-testen moeten uitvoeren; aanvankelijk zouden alle eenheidstests moeten mislukken.
3. Werk aan de taken in het bestand `Tasks.qs`.
4. Voer de `dotnet-test` opnieuw uit om uw codewijzigingen voor een taak te testen.

Voor het gemak bieden we ook een `tasks.json`-configuratie voor elke kata waarmee Visual Studio-code de build- en teststappen uit het opdrachtpalet kan uitvoeren.
Druk op Ctrl + Shift + P (of ⌘ + Shift + P op macOS) om het palet te openen en typ "Run Build Task" of "Run Test Task" en druk vervolgens op Enter.

# Bijdragende

Dit project verwelkomt bijdragen en suggesties. Zie [How can I Contribute?](.github/CONTRIBUTING.md) voor meer informatie.

# Gedragscode

Dit project heeft de [Microsoft Open Source Code of Conduct] (https://opensource.microsoft.com/codeofconduct/) aangenomen.
Zie voor meer informatie de [veelgestelde vragen over code of conduct] (https://opensource.microsoft.com/codeofconduct/faq/) of
neem contact op met [opencode@microsoft.com] (mailto: opencode@microsoft.com) met eventuele aanvullende vragen of opmerkingen.
