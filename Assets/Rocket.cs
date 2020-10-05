
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death; 
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive,Dying,Transcending}
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {   
        
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        

        //print(SceneManager.sceneCountInBuildSettings);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            ProcessInput();
        }
        

    }

    void OnCollisionEnter(Collision collision) {

        if (state != State.Alive) {

            return;

        }

            switch (collision.gameObject.tag)
        {

            case "Friendly":
                //
                break;
            case "Finish":
               state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                successParticle.Play();
                Invoke("LoadNextLevel",1f);
                break;
            default :
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticle.Play();
                Invoke("LoadFirstLevel",2f);
                break;
        }
    }

    void LoadNextLevel() {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {

            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void LoadFirstLevel() {

        SceneManager.LoadScene(0);
    }

    void Thrust() {

        if (Input.GetKey(KeyCode.Space))
        {

            rigidBody.AddRelativeForce(Vector3.up * ( mainThrust * Time.deltaTime ) );
            if (!audioSource.isPlaying )
            {

                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticle.Play();
        }
        else
        {

            audioSource.Stop();
            mainEngineParticle.Stop();
        }
    }
    void ProcessInput() {

        rigidBody.freezeRotation = true;
        float rotationSpeed = rcsThrust * Time.deltaTime;
        
        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;

    }
}

    
