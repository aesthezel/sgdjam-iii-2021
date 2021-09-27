using UnityEngine;
using UnityEngine.UI;

namespace Code.VFX
{
    public class BeamEffect : MonoBehaviour {

    [Header("Prefabs")]
    public GameObject[] beamLineRendererPrefab;
    public GameObject[] beamStartPrefab;
    public GameObject[] beamEndPrefab;

    private int _currentBeam = 0;

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f;
    public float textureScrollSpeed = 8f;
	public float textureLengthScale = 3;
    
    public Slider endOffSetSlider;
    public Slider scrollSpeedSlider;

    void Start()
    {
        if (endOffSetSlider)
            endOffSetSlider.value = beamEndOffset;
        if (scrollSpeedSlider)
            scrollSpeedSlider.value = textureScrollSpeed;
    }

    // Update is called once per frame
    // void Update()
    // {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     beamStart = Instantiate(beamStartPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //     beamEnd = Instantiate(beamEndPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //     beam = Instantiate(beamLineRendererPrefab[currentBeam], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        //     line = beam.GetComponent<LineRenderer>();
        // }
        
        // if (Input.GetMouseButtonUp(0))
        // {
        //     Destroy(beamStart);
        //     Destroy(beamEnd);
        //     Destroy(beam);
        // }

        // if (Input.GetMouseButton(0))
        // {
        //     if (UnityEngine.Camera.main is { })
        //     {
        //         Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        //         RaycastHit hit;
        //         if (Physics.Raycast(ray.origin, ray.direction, out hit))
        //         {
        //             var position = transform.position;
        //             Vector3 direction = hit.point - position;
        //             ShootBeamInDirirection(position, direction);
        //         }
        //     }
        // }
		
        /*
		if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextBeam();
        }

		if (Input.GetKeyDown(KeyCode.D))
		{
			nextBeam();
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			previousBeam();
		}
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            previousBeam();
        }
        */

    // }
    
    public void NextBeam()
    {
        if (_currentBeam < beamLineRendererPrefab.Length - 1)
            _currentBeam++;
        else
            _currentBeam = 0;
    }
	
    public void PreviousBeam()
    {
        if (_currentBeam > - 0)
            _currentBeam--;
        else
            _currentBeam = beamLineRendererPrefab.Length - 1; ;
    }
	

    public void UpdateEndOffset()
    {
        beamEndOffset = endOffSetSlider.value;
    }

    public void UpdateScrollSpeed()
    {
        textureScrollSpeed = scrollSpeedSlider.value;
    }

    void ShootBeamInDirirection(Vector3 start, Vector3 direction)
    {
        line.numCapVertices = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end;
        
        if (Physics.Raycast(start, direction, out var hit))
            end = hit.point - (direction.normalized * beamEndOffset);
        else
            end = transform.position + (direction * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        var sharedMaterial = line.sharedMaterial;
        sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }
}
}