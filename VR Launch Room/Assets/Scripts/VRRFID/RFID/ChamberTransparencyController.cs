using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberTransparencyController : MonoBehaviour
{
    [SerializeField] private Material SmoothMetal;
    [SerializeField] private Material BlueFoam;
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<MeshRenderer>().material;
        Debug.Log("Changing Render Mode");
        SetTransparency(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTransparency(float alpha = 1f)
    {
        if (alpha >= 1f)
        {
            StandardShaderUtils.ChangeRenderMode(SmoothMetal,StandardShaderUtils.BlendMode.Opaque);
            StandardShaderUtils.ChangeRenderMode(BlueFoam,StandardShaderUtils.BlendMode.Opaque);
        }
        else if(alpha >= 0)
        {
            StandardShaderUtils.ChangeRenderMode(SmoothMetal,StandardShaderUtils.BlendMode.Transparent);
            StandardShaderUtils.ChangeRenderMode(BlueFoam,StandardShaderUtils.BlendMode.Transparent);
            Color tmp = SmoothMetal.color;
            tmp.a = alpha;

            SmoothMetal.color = tmp;
            
            tmp = BlueFoam.color;
            tmp.a = alpha;

            BlueFoam.color = tmp;
        }
        else
        {
            Debug.Log("Alpha value cannot be < 0");
        }
    }
}
