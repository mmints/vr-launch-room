using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource stepSource;
    // tutorial clips
    [SerializeField] private AudioClip welcome;
    [SerializeField] private AudioClip letsGo;
    [SerializeField] private AudioClip sweepSettings;
    [SerializeField] private AudioClip openChamber;
    [SerializeField] private AudioClip insideChamber;
    [SerializeField] private AudioClip chooseTag;
    [SerializeField] private AudioClip niceTag;
    [SerializeField] private AudioClip closeChamber;
    [SerializeField] private AudioClip closeChamberFinished;
    [SerializeField] private AudioClip measurementIsRunning;
    [SerializeField] private AudioClip measurementFinished;
    [SerializeField] private AudioClip measurementFinished2;
    [SerializeField] private AudioClip somethingMissing;
    [SerializeField] private AudioClip readRange;
    [SerializeField] private AudioClip orientationDescription;
    [SerializeField] private AudioClip orientationStarted;
    [SerializeField] private AudioClip orientationFinished;
    [SerializeField] private AudioClip congratulation;
    [SerializeField] private AudioClip wellDone;
    [SerializeField] private AudioClip step;


    private bool dontRepeat = false;

    public bool DontRepeat
    {
        get => dontRepeat;
        set => dontRepeat = value;
    }

    public void PlayStepSound()
    {
        stepSource.clip = step;
        stepSource.Play();
    }
    
    // Unfortunately the event system is a bit unflexible when it comes to parameters.
    
    public void RepeatLastSound()
    {
        if(!audioSource.isPlaying && !dontRepeat)
            audioSource.Play();
    }
    public IEnumerator PlayWelcome()
    {
        audioSource.clip = welcome;
        audioSource.Play();

        yield return new WaitUntil(FinishedPlaying);
    }
    public IEnumerator PlayLetsGo()
    {
        audioSource.clip = letsGo;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlaySweepSettings()
    {
        audioSource.clip = sweepSettings;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayOpenChamber()
    {
        audioSource.clip = openChamber;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayInsideChamber()
    {
        audioSource.clip = insideChamber;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayChooseTag()
    {
        audioSource.clip = chooseTag;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayNiceTag()
    {
        audioSource.clip = niceTag;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayCloseChamber()
    {
        audioSource.clip = closeChamber;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayCloseChamberFinished()
    {
        audioSource.clip = closeChamberFinished;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayMeasurementIsRunning()
    {
        audioSource.clip = measurementIsRunning;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayMeasurementIsFinished()
    {
        audioSource.clip = measurementFinished;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayMeasurementIsFinished2()
    {
        audioSource.clip = measurementFinished2;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayReadRange()
    {
        audioSource.clip = readRange;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayOrientationDescription()
    {
        audioSource.clip = orientationDescription;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayOrientationStarted()
    {
        audioSource.clip = orientationStarted;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayOrientationFinished()
    {
        audioSource.clip = orientationFinished;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayCongratulation()
    {
        audioSource.clip = congratulation;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }
    
    public IEnumerator PlayWellDone()
    {
        audioSource.clip = wellDone;
        audioSource.Play();
        
        yield return new WaitUntil(FinishedPlaying);
    }

    public bool FinishedPlaying()
    {
        return !audioSource.isPlaying;
    }
}
