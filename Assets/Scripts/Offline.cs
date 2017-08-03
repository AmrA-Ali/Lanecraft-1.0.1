public class Offline  {

	public static Map[] Maps;
	public static bool MapsReady=false;

	public static void GetMaps(){
		MapsReady=false;
		Maps = Map.FetchMapsInfoOffline ();
		MapsReady=true;
	}
}
