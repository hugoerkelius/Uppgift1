# Uppgift1
Jag valde att nyttja switch case till stor del i mitt program för i min nuvarande kompetens anser jag det tydligt att hantera för att skifta mellan olika funktioner i menyer. Jag insåg under projektets gång att jag kunde nyttjat mig mer av bool i mina switchcases för att skapa ett bättre flöde. Så att jag exempelvis inte breakar till start vid inmatning av felaktig data i trade funktionen.

Jag inser också begräsningar i min trading.cs, jag hade kunna komprimerat min kod och skapat metoder för att hämta respekektive data i min program.cs. Däremot fick jag aldrig till det under mitt arbete så valde att hårdkoda in det. 

Det är ett genomgående problem för mig i min program.cs, jag hade kunnat komprimera hela min kod avsevärt men pågrund av begränsningar i min kunskap valde jag att nyttja det olika resurser jag kände mig bekväm med att använda. Listor ansåg jag mest rimliga för att enkelt kunna samla data för de olika entiterna i mitt program. Det blev enkelt läsa in och ut korrekt data i mitt program genom att använda listor. Arrayer använde jag enbart för att med enkelhet kunna läsa in data från mina sparade csv filer in i programmet när jag startatede det. 

Komposition och inheritence valde jag ej att nyttja för jag har inte erfarenhet av vad det är. 

Kör programmet genom att först välja om du vill skapa en användare genom att skriva in 1 eller 2. 

Skapar du en användare behöver du välja ett unikt användarnamn, ditt namn och sedan ett password som behöver ha minst 8 karaktärer.

Vid inloggning behöver du enbart infoga ditt användarnamn och lösenord.

Därefter kommer du till trader menyn där du kan välja att 
1. Lista dina items
2. Lägga till items i ditt inventory
- Här ska du välja ett namn på ditt item
- Hur många av de itemet du har
- En kort beskrivning av ditt item
- Därefter sparas ditt item till användarens list och i en CSV fil
3. Skapa en trade om det finns tillgängliga traders med någonting i deras inventory
- Inledningsvis får du en lista på alla traders som är registrerade och en lista på vad respektive traders har för items, hur många och en kort beskrivning.
- Välj därefter vilken trader du vill byta med genom att infoga dennes användarnamn som syns i listan
- Välj slutligen vilka items som ska tradeas
4. Lista alla trades som är tillgängliga för dig
- Här ser du en lista med ett index nr. framför de trades som är skickade till dig
- Välj trade genom att infoga respektive index nr. 
- Acceptera trade genom att skriva accept och deny (Inget egentligen) för att deny trade
5. Lista alla genomförda trades, där ser du om de är accepted eller denied.
- Här ser du en lista över alla genomförda trades
- Här visas vad det är för items och vem du har tradeat med för vilka items.

