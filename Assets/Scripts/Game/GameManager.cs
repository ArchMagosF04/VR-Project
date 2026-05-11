using System.Collections.Generic;
using UnityEngine;
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
        fullComplete = Solution.Length;
        completionProgress = 0f;
        progressBar.fillAmount = completionProgress / fullComplete;
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    //        if (hit.collider && hit.collider.TryGetComponent<StarObject>(out StarObject star))
    //        {
    //            if (star.AnchorPoint == null) return;

    //            if (SelectedStar == null || SelectedStar == star)
    //            {
    //                SelectedStar = star;
    //                return;
    //            }

    //            if (ConnectionExist(SelectedStar, star)) return;

    //            LineRenderer newLine = Instantiate(starLanePrefab);

    //            newLine.SetPosition(0, SelectedStar.transform.position);
    //            newLine.SetPosition(1, star.transform.position);

    //            StarConnection newConnection = new StarConnection(SelectedStar.AnchorPoint, star.AnchorPoint, newLine);

    //            star.AddNewConnection(newConnection);
    //            SelectedStar.AddNewConnection(newConnection);

    //            CurrentConnections.Add(newConnection);

    //            AssessConnection(newConnection);

    //            SelectedStar = null;
    //        }
    //    }
    //}

    public void SelectStar(StarObject star)
    {
        if (star.AnchorPoint == null) return;

        if (SelectedStar == null || SelectedStar == star)
        {
            SelectedStar = star;
            return;
        }

        if (ConnectionExist(SelectedStar, star)) return;

        LineRenderer newLine = Instantiate(starLanePrefab);

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

            Destroy(connection.lineConnect.gameObject);

            for (int i = 0; i < Solution.Length; i++)
            {
                if (Solution[i].Solved && Solution[i].correctConnection == connection)
                {
                    Solution[i].Solved = false;
                    Solution[i].correctConnection = null;
                }
            }

            completionProgress = Mathf.Clamp(CorrectConnections.Count - WrongConnections.Count, 0f, fullComplete);
            progressBar.fillAmount = completionProgress / fullComplete;
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
    }
}
