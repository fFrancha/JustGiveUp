using System.Net.Mime;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public void Jugar()
   {
        SceneManager.LoadScene(1);
   }

   public void Salir()
   {
        Application.Quit();
   }

   public void MenuPrincipal()
   {
     SceneManager.LoadScene(0);
   }
}
