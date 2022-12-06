
using System;
using System.Collections.Generic;
using System.Collections;

namespace DeepSpace
{

	class Estrategia
	{
		
		private int profundidad(ArbolGeneral<Planeta> arbol)
		{
			if(arbol.esHoja() && !arbol.getDatoRaiz().EsPlanetaDeLaIA()) 
			{
				return -1; 
			}
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA()) 
			{
				return 0;
			}
			else 
			{
				foreach(var hijo in arbol.getHijos()) 
				{
					int valor = profundidad(hijo); 
					if(valor >= 0) 
					{
						return ++valor; 
					}
				}
				return -1;
			}
			
	
		}
		
		public String Consulta1( ArbolGeneral<Planeta> arbol)
		{
			return "La distancia entre el Planeta Bot y la Raiz es de: " + profundidad(arbol) + " planetas"; 
		}
		
		
	    public string Consulta2( ArbolGeneral<Planeta> arbol)
	    {
	    	ArrayList planetas = new ArrayList(); 
	    	Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>(); 
	    	cola.encolar(arbol); 
	    	while(!cola.esVacia()) 
	    	{
	    		ArbolGeneral<Planeta> actual = cola.desencolar(); 
	    		if(actual.getDatoRaiz().EsPlanetaDeLaIA()) 
	    			{
	    			
	    				string res = "Planetas descendientes del Bot: "; 
	    				if(actual.esHoja()) 
						{
							return "El planeta del Bot no tiene descendientes"; 
						}
	    				foreach(var planeta in SubArbolIA(actual,planetas)) 
	    				{
	    					res += planeta + " "; 
						}
	    				return res; 
	    			}
	    			else 
	    			{
	    				foreach(var hijo in actual.getHijos()) 
	    				{
	    					cola.encolar(hijo); 
	    				}
	    			}
	    		
	    		
	    	}
	    	return ""; 
	  
	    }
		

	    
		private ArrayList SubArbolIA(ArbolGeneral<Planeta> PlanetaIA, ArrayList lista)
		{
			if(PlanetaIA != null) 
			{
				Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>(); 
				cola.encolar(PlanetaIA); 
				ArbolGeneral<Planeta> aux; 
				while(!cola.esVacia()) 
				{
					aux = cola.desencolar(); 
					lista.Add(aux.getDatoRaiz().population); 
					if(aux.getHijos() != null) 
					{

						foreach(var hi in aux.getHijos()) 
						{
							cola.encolar(hi); 
						}
					}
				
				}
			}
			lista.RemoveAt(0); 
			return lista; 
			
		}
		
		public String Consulta3( ArbolGeneral<Planeta> arbol)
		{
			
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>(); 
			
			ArbolGeneral<Planeta> actual; 
			cola.encolar(arbol); 
			cola.encolar(null);		
			
			int cantPlanetasXNivel = 0;
			int cantPobXNivel = 0;
			int cantPobTotal = 0;          
			int nivel = 0;
			int PromPobXNivel = 0;
			string res = "";

			while (!cola.esVacia()) 
			{
				actual = cola.desencolar(); 
				
				if (actual != null) 
				{       
					cantPlanetasXNivel++;  
					cantPobXNivel += actual.getDatoRaiz().Poblacion(); 
					cantPobTotal += actual.getDatoRaiz().Poblacion();  
					PromPobXNivel = cantPobXNivel / cantPlanetasXNivel; 
					foreach (var hijo in actual.getHijos())
					{
						cola.encolar(hijo); 
					}					
				}
				else 
				{
					res += "Nivel " + nivel + ". Poblacion: " + cantPobXNivel + "  Promedio de poblacion " +  " : " + PromPobXNivel + "\n"; 
					cantPlanetasXNivel = 0; 
					cantPobXNivel = 0;      
					nivel++;                
					PromPobXNivel = 0;      
					
					if (!cola.esVacia())
					{
						cola.encolar(null); 
					}
				}
				
			}

			return "Poblacion y Promedio por nivel:\n" + res + "Poblacion Total: " + cantPobTotal; 
			
		}
	
	
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			List<ArbolGeneral<Planeta>> Attack = new List<ArbolGeneral<Planeta>>();
			List<ArbolGeneral<Planeta>> CaminoBotHaciaJugador = Camino_Ataque(Camino_Raiz_A_Bot(arbol),Camino_Raiz_A_Jugador(arbol),Attack);
			List<ArbolGeneral<Planeta>> CaminoJugador = Camino_Raiz_A_Jugador(arbol);
			
			foreach(var planets in CaminoJugador)
			{
				if(planets.getDatoRaiz().EsPlanetaDeLaIA())
				{
					for(int x=0;x<CaminoBotHaciaJugador.Count;x++)
					{
						if(CaminoBotHaciaJugador[x].getDatoRaiz().EsPlanetaDeLaIA() && CaminoBotHaciaJugador[x+1].getDatoRaiz().EsPlanetaDeLaIA())
						{
							CaminoBotHaciaJugador[x] = CaminoBotHaciaJugador[x+1];
							CaminoBotHaciaJugador.RemoveAt(x+1);
						}
					}
				}
	
			}
			
			if(CaminoBotHaciaJugador[0].getDatoRaiz().EsPlanetaDeLaIA() && !CaminoBotHaciaJugador[1].getDatoRaiz().EsPlanetaDeLaIA())
			{
				Movimiento ataque = new Movimiento(CaminoBotHaciaJugador[0].getDatoRaiz(),CaminoBotHaciaJugador[1].getDatoRaiz());
				if(CaminoBotHaciaJugador[0].getDatoRaiz().population < CaminoBotHaciaJugador[1].getDatoRaiz().population)
				{
					
					foreach(var hijo in CaminoBotHaciaJugador[0].getHijos())
					{
						if(hijo.getDatoRaiz().EsPlanetaDeLaIA())
						{
							Movimiento refuerzo = new Movimiento(hijo.getDatoRaiz(),CaminoBotHaciaJugador[0].getDatoRaiz());
							foreach(var hi in hijo.getHijos())
							{
								if(hi.getDatoRaiz().EsPlanetaDeLaIA() && hi.getDatoRaiz().Poblacion() > hijo.getDatoRaiz().Poblacion())
								{
									Movimiento refuerzoPotente = new Movimiento(hi.getDatoRaiz(),hijo.getDatoRaiz());
									return refuerzoPotente;
								}
							}
							
							return refuerzo;
						}
						if(hijo.getDatoRaiz().EsPlanetaDelJugador())
						{
							if(CaminoJugador.Count >= 3)
							{
								Movimiento AyudameATerminar = new Movimiento(CaminoJugador[CaminoJugador.Count -3].getDatoRaiz(),CaminoBotHaciaJugador[0].getDatoRaiz());
								if(CaminoJugador.Count>=4)
								{
									if(CaminoJugador[CaminoJugador.Count-3].getDatoRaiz().Poblacion() < CaminoJugador[CaminoJugador.Count -4].getDatoRaiz().Poblacion()&& CaminoJugador[CaminoJugador.Count -4].getDatoRaiz().EsPlanetaDeLaIA())
									{
										Movimiento AyudameATerminar2 = new Movimiento(CaminoJugador[CaminoJugador.Count -4].getDatoRaiz(),CaminoJugador[CaminoJugador.Count -3].getDatoRaiz());
										return AyudameATerminar2;
									}
								}
								return AyudameATerminar;
							}
						}
						
					}


				}
				
				
				
				
				
				return ataque;
			}

			
			return null;
		}
		
		

		
		
		

		
		
		
		
		
		private List<ArbolGeneral<Planeta>> Camino_Ataque(List<ArbolGeneral<Planeta>> caminobot, List<ArbolGeneral<Planeta>> caminojugador, List<ArbolGeneral<Planeta>> Ataque)
		{
			ArbolGeneral<Planeta> aux = null; 
			for(int x = caminobot.Count -1;x>=0;x--) 
			{
				if(caminobot[x] == LCA(caminobot,caminojugador,aux)) 
				{
					break;
				}
				Ataque.Add(caminobot[x]); 
				
			}
			
			bool añadir = false; 
			for(int x = 0;x<caminojugador.Count;x++) 
			{
				if(caminojugador[x] == LCA(caminobot,caminojugador,aux)){ 
					añadir = true; 
				}
				if(añadir) 
				{
					Ataque.Add(caminojugador[x]); 
				}
			}
		
			return Ataque; 
			
		}


		private ArbolGeneral<Planeta> LCA(List<ArbolGeneral<Planeta>> caminobot, List<ArbolGeneral<Planeta>> caminojugador, ArbolGeneral<Planeta> LCA)
				{
					Cola<ArbolGeneral<Planeta>> colabot = new Cola<ArbolGeneral<Planeta>>();      
					Cola<ArbolGeneral<Planeta>> colajugador = new Cola<ArbolGeneral<Planeta>>();  
					for(int x = 0; x<caminobot.Count;x++){colabot.encolar(caminobot[x]);}         
					for(int x = 0; x<caminojugador.Count;x++){colajugador.encolar(caminojugador[x]);} 
					while(!colabot.esVacia() && !colajugador.esVacia()) 
						{
							ArbolGeneral<Planeta> auxb = colabot.desencolar(); 
							ArbolGeneral<Planeta> auxj = colajugador.desencolar(); 
							if(auxb == auxj) 
							{
							LCA = auxb; 
							}
					}
						return LCA; 
					}

		
		public List<ArbolGeneral<Planeta>> Camino_Raiz_A_Bot(ArbolGeneral<Planeta> arbol)
		{
			List<ArbolGeneral<Planeta>> camino = new List<ArbolGeneral<Planeta>>();
			return Raiz_A_Bot(arbol,camino);
		}
		public List<ArbolGeneral<Planeta>> Camino_Raiz_A_Jugador(ArbolGeneral<Planeta> arbol)
		{
			List<ArbolGeneral<Planeta>> camino = new List<ArbolGeneral<Planeta>>();
			return Raiz_A_Jugador(arbol,camino);
		}
		
		private List<ArbolGeneral<Planeta>> Raiz_A_Bot(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> camino) 
		{
			camino.Add(arbol); 
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA()) 
			{
				return camino; 
			}
			else 
			{
				
				foreach(var hi in arbol.getHijos()) 
				{
					List<ArbolGeneral<Planeta>> aux = Raiz_A_Bot(hi,camino); 
					if(aux != null) 
					{
						return aux; 
					}
					
				}
				camino.RemoveAt(camino.Count -1); 
			}
			return null; 
		
		}
		
		private List<ArbolGeneral<Planeta>> Raiz_A_Jugador(ArbolGeneral<Planeta> arbol, List<ArbolGeneral<Planeta>> camino) 
		{
			camino.Add(arbol); 
			if(arbol.getDatoRaiz().EsPlanetaDelJugador()) 
			{
				return camino; 
			}
			else 
			{
				
				foreach(var hi in arbol.getHijos()) 
				{
					List<ArbolGeneral<Planeta>> aux = Raiz_A_Jugador(hi,camino); 
					if(aux != null) 
					{
						return aux; 
					}
					
				}
				camino.RemoveAt(camino.Count -1); 
			}
			return null; 
		}

	}
}
