using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public StarObject SelectedStar;
    public LineRenderer starLanePrefab;

    public StarConnectionAnswer[] Solution;

    public List<StarConnection> CurrentConnections = new List<StarConnection>();
    public List<StarConnection> CorrectConnections = new List<StarConnection>();
    public List<StarConnection> WrongConnections = new List<StarConnection>();

    public float completionProgress;
    public float fullComplete;

    public Image progressBar;

    [SerializeField] private ParticleSystem starSelectedParticles;

    [SerializeField] private string nextScene;

    [SerializeField] private BlackHole blackHole;

    [SerializeField] private GameObject[] objectsToAllwaysDisable;
    [SerializeField] private GameObject[] objectsToDisableOnDefeat;
    [SerializeField] private MeshRenderer[] meshRenderers;
    [SerializeField] private Material starMaterial;

    [SerializeField] private Canvas victoryCanvas;
    [SerializeField] private Canvas defeatCanvas;

    [SerializeField] private CanvasGroup finalImage;

    [SerializeField] private Transform constelationObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        finalImage.alpha = 0f;
        starMaterial.SetFloat("_StarNumber", 100);

        fullComplete = Solution.Length;
        completionProgress = 0f;
        progressBar.fillAmount = completionProgress / fullComplete;

        if (victoryCanvas) victoryCanvas.enabled = false;
        if (defeatCanvas) defeatCanvas.enabled = false;
    }

    private void OnDisable()
    {
        starMaterial.SetFloat("_StarNumber", 100);
    }

    private void OnDestroy()
    {
        starMaterial.SetFloat("_StarNumber", 100);
    }

    public void SelectStar(StarObject star)
    {
        if (star.AnchorPoint == null)
        {
            Debug.Log("Star has no anchor");
            return;
        }

        if (SelectedStar == null || SelectedStar == star)
        {
            if (!star.HasMaxConnections())
            {
                SelectedStar = star;
                starSelectedParticles.transform.position = star.transform.position;
                starSelectedParticles.Play();
                Debug.Log("Set Selected Star");
            }
            else
            {
                Debug.Log("Has max");
            }
            return;
        }

        if (SelectedStar.HasMaxConnections() || star.HasMaxConnections())
        {
            Debug.Log("Star has max connections");
            return;
        }

        if (ConnectionExist(SelectedStar, star))
        {
            Debug.Log("Connection Exists between this stars already.");
            return;
        }

        starSelectedParticles.transform.position = star.transform.position;
        starSelectedParticles.Play();

        LineRenderer newLine = Instantiate(starLanePrefab, constelationObject);

        newLine.SetPosition(0, SelectedStar.transform.position);
        newLine.SetPosition(1, star.transform.position);

        StarConnection newConnection = new StarConnection(SelectedStar.AnchorPoint, star.AnchorPoint, newLine);

        star.AddNewConnection(newConnection);
        SelectedStar.AddNewConnection(newConnection);

        CurrentConnections.Add(newConnection);

        AssessConnection(newConnection);

        SelectedStar = null;
    }

    private bool ConnectionExist(StarObject star1, StarObject star2)
    {
        for (int i = 0; i < CurrentConnections.Count; i++)
        {
            if ((CurrentConnections[i].StarA == star1 || CurrentConnections[i].StarA == star2) &&
                (CurrentConnections[i].StarB == star1 || CurrentConnections[i].StarB == star2))
            {
                return true;
            }
        }

        return false;
    }

    public void DeleteConnection(StarConnection connection)
    {
        if (CurrentConnections.Contains(connection))
        {
            CurrentConnections.Remove(connection);
            if (CorrectConnections.Contains(connection)) CorrectConnections.Remove(connection);
            if (WrongConnections.Contains(connection)) WrongConnections.Remove(connection);

            connection.StarA.connections.Remove(connection);
            connection.StarB.connections.Remove(connection);

            if (connection.lineConnect != null) Destroy(connection.lineConnect.gameObject);

            for (int i = 0; i < Solution.Length; i++)
            {
                if (Solution[i].Solved && Solution[i].correctConnection == connection)
                {
                    Solution[i].Solved = false;
                    Solution[i].correctConnection = null;
                }
            }

            completionProgress = Mathf.Clamp(CorrectConnections.Count - WrongConnections.Count, 0f, fullComplete);
            if (progressBar != null) progressBar.fillAmount = completionProgress / fullComplete;
        }
    }

    public void AssessConnection(StarConnection connection)
    {
        bool isCorrect = false;

        for (int i = 0; i < Solution.Length; i++)
        {
            if ((connection.PointA.Equals(Solution[i].PointA) || connection.PointA.Equals(Solution[i].PointB)) && 
                (connection.PointB.Equals(Solution[i].PointB) || connection.PointB.Equals(Solution[i].PointA)))
            {
                isCorrect = true;
                Solution[i].Solved = true;
                Solution[i].correctConnection = connection;
                break;
            }
        }

        if (isCorrect) CorrectConnections.Add(connection);
        else WrongConnections.Add(connection);

        completionProgress = Mathf.Clamp((float)CorrectConnections.Count - ((float)WrongConnections.Count / 2), 0f, fullComplete);
        progressBar.fillAmount = completionProgress / fullComplete;

        if (completionProgress == fullComplete)
        {
            Invoke("OnGameWon", 2f);
        }
    }

    public void OnGameLost()
    {
        blackHole.HideBlackhole();

        foreach (GameObject obj in objectsToDisableOnDefeat)
        {
            obj.SetActive(false);
        }

        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }

        starMaterial.SetFloat("_StarNumber", 0);

        defeatCanvas.enabled = true;
    }

    public void OnGameWon()
    {
        blackHole.StopBlackHole();

        finalImage.DOFade(1, 1f).OnComplete(WinSequence);

        foreach (GameObject obj in objectsToAllwaysDisable)
        {
            obj.SetActive(false);
        }
    }

    private void WinSequence()
    {
        blackHole.DestroyBlackHole();

        foreach (StarConnection connection in CurrentConnections)
        {
            if (connection.lineConnect != null) connection.lineConnect.enabled = false;
        }

        constelationObject.DOLocalMoveX(2f, 5f).SetEase(Ease.OutQuad).OnComplete(() => victoryCanvas.enabled = true);
    }

    public void ReloadLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
