using UnityEngine;

public class QuizClick : MonoBehaviour
{
    public string markerName;
    private QuizSystem quiz;
    private Camera cam;

    void Start()
    {
        quiz = FindObjectOfType<QuizSystem>();
        cam = Camera.main;

        if (quiz == null)
            Debug.LogError("QuizSystem tidak ditemukan di scene!");
    }

    void Update()
    {
        Vector2 tapPos;

        // PC (mouse)
        if (Input.GetMouseButtonDown(0))
        {
            tapPos = Input.mousePosition;
            CheckRaycast(tapPos);
            return;
        }

        // Android (touch)
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                tapPos = t.position;
                CheckRaycast(tapPos);
            }
        }
    }

    void CheckRaycast(Vector2 pos)
    {
        if (cam == null) cam = Camera.main;

        Ray ray = cam.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f))
        {
            if (hit.transform == transform)
            {
                quiz.OpenQuiz(markerName);
            }
        }
    }
}
