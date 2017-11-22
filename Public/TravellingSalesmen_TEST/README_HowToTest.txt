Hogyan tesztelj :
1.	GeneticAlgorithm osztályon belül lehet módosítani
	5 értéket:
		generationsNumber
		populationSize (4gyel osztható)
		mutationProbability
		firstchildmutationTrue
		secondchildMutationTrue
2.	Hogy gyorsabb legyen átírhatod a Thread.Sleep idejét
	a Coordinator osztály 87es sorjában a sleepTime-ot
	(RunAlgoritmhThrough függvény elsõ sora)3
3.	Futtass egy GA algot egy GA conffal
4.	Zárd be, csináld újra a 2es és 4es pontokat
5.	Az eredmény a Tests mappában lesz található

--------------
Megjegyzés:
Futtathatsz többször ugyan olyan bemenetekkel, ilyenkor a létezõ txt-be írja bele az eredményt a következõ sorba

CSINÁJL ÚJ CONFIGURÁCIÓKAT IS !!!

Csinálj újabb gráfokat:
- a koordiátákat space-el elválasztva
- a neve a kövi formátumnak tegyen eleget:
	graph*_**.txt
	*  -> hány csúcs van a gráfban
	** -> hanyadik létezõ gráf a *csúcsúak közül
példa: graph10_4.txt,
	azaz ez 10 csúcsú, és eddig volt 3db 10csúcsú

Csinálj újabb agenteket:
- a kezdeti helyeket space-el elválasztva (ahány ügynök van)
- a neve a kövi formátumnak tegyen eleget:
	agents*_**.txt
	*  -> hány agent van
	** -> hanyadik fájl ezzel az agent számmal
példa: agents3_5.txt,
	azaz 3agent van, és már volt eddig 4 ilyen 3tagú fájl

Csinálj újabb confokata következõ szabályokkal:
- futtasd a programot
- írd be a gráf fájlnevét (.txt vel együtt)
- írd be az agent fájlnevét (.txt vel együtt)
- VIGYÁZZ hogy ne legyen több agent mint csúcs !!!

- mentsd el a következõ néven:
gráfFájlnév + "_" + agentFájlnév

pl: graph13_3_agents6_2