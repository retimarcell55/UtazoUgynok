Hogyan tesztelj :
1.	GeneticAlgorithm oszt�lyon bel�l lehet m�dos�tani
	5 �rt�ket:
		generationsNumber
		populationSize (4gyel oszthat�)
		mutationProbability
		firstchildmutationTrue
		secondchildMutationTrue
2.	Hogy gyorsabb legyen �t�rhatod a Thread.Sleep idej�t
	a Coordinator oszt�ly 87es sorj�ban a sleepTime-ot
	(RunAlgoritmhThrough f�ggv�ny els� sora)3
3.	Futtass egy GA algot egy GA conffal
4.	Z�rd be, csin�ld �jra a 2es �s 4es pontokat
5.	Az eredm�ny a Tests mapp�ban lesz tal�lhat�

--------------
Megjegyz�s:
Futtathatsz t�bbsz�r ugyan olyan bemenetekkel, ilyenkor a l�tez� txt-be �rja bele az eredm�nyt a k�vetkez� sorba

CSIN�JL �J CONFIGUR�CI�KAT IS !!!

Csin�lj �jabb gr�fokat:
- a koordi�t�kat space-el elv�lasztva
- a neve a k�vi form�tumnak tegyen eleget:
	graph*_**.txt
	*  -> h�ny cs�cs van a gr�fban
	** -> hanyadik l�tez� gr�f a *cs�cs�ak k�z�l
p�lda: graph10_4.txt,
	azaz ez 10 cs�cs�, �s eddig volt 3db 10cs�cs�

Csin�lj �jabb agenteket:
- a kezdeti helyeket space-el elv�lasztva (ah�ny �gyn�k van)
- a neve a k�vi form�tumnak tegyen eleget:
	agents*_**.txt
	*  -> h�ny agent van
	** -> hanyadik f�jl ezzel az agent sz�mmal
p�lda: agents3_5.txt,
	azaz 3agent van, �s m�r volt eddig 4 ilyen 3tag� f�jl

Csin�lj �jabb confokata k�vetkez� szab�lyokkal:
- futtasd a programot
- �rd be a gr�f f�jlnev�t (.txt vel egy�tt)
- �rd be az agent f�jlnev�t (.txt vel egy�tt)
- VIGY�ZZ hogy ne legyen t�bb agent mint cs�cs !!!

- mentsd el a k�vetkez� n�ven:
gr�fF�jln�v + "_" + agentF�jln�v

pl: graph13_3_agents6_2