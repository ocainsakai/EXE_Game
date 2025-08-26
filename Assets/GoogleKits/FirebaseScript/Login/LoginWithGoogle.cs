using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Google;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoginWithGoogle : MonoBehaviour
{
    public string GoogleAPI = "1003971093115-1deuoh1q6v2j5lp53s20kant827p9h45.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    FirebaseAuth auth;
    FirebaseUser user;

    public TextMeshProUGUI UserName;
    public TextMeshProUGUI UserEmail;

    private bool isGoogleSignInInitialzed = false;

    private void Start()
    {
        InitFirebase();
    }

    private void InitFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login()
    {
        if (!isGoogleSignInInitialzed)
        {
            configuration = new GoogleSignInConfiguration
            {
                WebClientId = GoogleAPI,
                RequestIdToken = true,
                RequestEmail = true,
                //ForceTokenRefresh = true
            };
            GoogleSignIn.Configuration = configuration;
            isGoogleSignInInitialzed = true;
        }
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UserName.text = "Sign-In canceled.";
                UserEmail.text = "Sign-In canceled.";
                Debug.Log("Google Sign-In was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                UserName.text = "Sign-In failed.";
                UserEmail.text = "Sign-In failed.";
                Debug.LogError($"Google Sign-In failed: {task.Exception}");
                return;
            }

            GoogleSignInUser googleUser = task.Result;
            Credential credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
            {
                if (authTask.IsCanceled)
                {
                    UserName.text = "Firebase Sign-In canceled.";
                    UserEmail.text = "Firebase Sign-In canceled.";
                    Debug.Log("Firebase Sign-In was canceled.");
                    return;
                }
                if (authTask.IsFaulted)
                {
                    UserName.text = "Firebase Sign-In failed.";
                    UserEmail.text = "Firebase Sign-In failed.";
                    Debug.LogError($"Firebase Sign-In failed: {authTask.Exception}");
                    return;
                }

                user = authTask.Result;
                Debug.Log($"Firebase Sign-In succeeded: {user.DisplayName}");
                UserName.text = user.DisplayName;
                UserEmail.text = user.Email;
            });
        });
    }
}
