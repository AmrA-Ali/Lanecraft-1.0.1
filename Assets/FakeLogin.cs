using UnityEngine;
using UnityEngine.UI;


public class FakeLogin : MonoBehaviour {
	public string Username;
	public string Password;

	void Start()
	{ GetComponent<Button>().onClick.AddListener(delegate { Login(); }); }
	
	
	void Login(){
		new GameSparks.Api.Requests.RegistrationRequest()
		// this login method first attempts a registration //
		// if the player is not new, we will be able to tell as the registrationResponse has a bool 'NewPlayer' which we can check //
		// for this example we use the user-name was the display name also //
			.SetDisplayName(Username)
			.SetUserName(Username)
			.SetPassword(Password)
			.Send((regResp) => {
				if(!regResp.HasErrors){ // if we get the response back with no errors then the registration was successful
					Debug.Log("GSM| Registration Successful..."); 
				}else{
					// if we receive errors in the response, then the first thing we check is if the player is new or not
					if(!(bool)regResp.NewPlayer) // player already registered, lets authenticate instead
					{
						Debug.LogWarning("GSM| Existing User, Switching to Authentication");
						new GameSparks.Api.Requests.AuthenticationRequest()
							.SetUserName(Username)
							.SetPassword(Password)
							.Send((authResp) => {
								if(!authResp.HasErrors){
									Debug.Log("Authentication Successful...");
								}else{
									Debug.LogWarning("GSM| Error Authenticating User \n"+authResp.Errors.JSON);
								}
							});
					}else{
						Debug.LogWarning("GSM| Error Authenticating User \n"+regResp.Errors.JSON); // if there is another error, then the registration must have failed
					}
				}
			});
	}
}
