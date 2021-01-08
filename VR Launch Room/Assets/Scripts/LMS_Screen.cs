using System.Collections;
using Moodle;
using TMPro;
using UnityEngine;

public class LMS_Screen : MonoBehaviour
{
    public MoodleUser moodleUser;
    public MoodleConnector moodleConnector;
    
    public TextMeshProUGUI screenOutput;
    public Renderer profileImage;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SynchronizeMoodleData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SynchronizeMoodleData()
    {
        yield return new WaitUntil(() => moodleConnector.Ready);
        
        UpdateScreen();
        UpdateProfileImage();
    }
    
    void UpdateScreen()
    {
        string text = "Hallo " +  moodleUser.fullName + " dein Username lautet: " + moodleUser.username;
        text += ". Außerdem bist du in folgenden Kursen eingeschrieben und hast Zugriff auf entsprechende Activities:\n";
        foreach (var enrolledCourse in moodleUser.enrolledCourses)
        {
            text += enrolledCourse.FullName + "\n";
            foreach (var activity in enrolledCourse.activities)
            {
                text += "      - " + activity.Name + " type: " + activity.ModName + "\n";
            }
            
        }
        
        screenOutput.text = text;
    }
    
    void UpdateProfileImage()
    {
        profileImage.material.mainTexture = moodleUser.profileImage;
    }
}
