using System.Net.Mime;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;

    public void Update()
    {
    Pausa();
    Reanudar();
    }

    public void Pausa()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            menuPausa.SetActive(true);
        }
    }
    
    public void Reanudar()
    {
        Time.timeScale = 1f;
        menuPausa.SetActive(false);
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