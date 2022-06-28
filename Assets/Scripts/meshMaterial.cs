using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshMaterial : MonoBehaviour
{
    public control.materialType myMaterial;
    public int timeOffset;
    MeshRenderer mesh;
    public void initialize(int newTimeOffset = 0)
    {
        Material materialToSet = control.materialsList[myMaterial].material;
        mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = materialToSet;
        timeOffset = newTimeOffset;
        if(timeOffset > 0)
        {
            Color transparentColor = mesh.material.color;
            //transparentColor.a = 0.25f;
            transparentColor.g = transparentColor.g *2;
            mesh.material.color = transparentColor;
            //ChangeRenderMode(mesh.material, 2);
        }
        if (timeOffset < 0)
        {
            Color transparentColor = mesh.material.color;
            //transparentColor.a = 0.25f;
            transparentColor.r = 10+transparentColor.r * 2;
            mesh.material.color = transparentColor;
            //ChangeRenderMode(mesh.material, 2);
        }
        if (timeOffset == 0)
        {
            //ChangeRenderMode(mesh.material, 0);
        }
    }

    void ChangeRenderMode(Material standardShaderMaterial, int blendMode)
    {
        switch (blendMode)
        {
            case 0: //opaque
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = -1;
                break;
            case 1: //cuttout
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                standardShaderMaterial.SetInt("_ZWrite", 1);
                standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 2450;
                break;
            case 2: //fade
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
            case 3://transparent
                standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                standardShaderMaterial.SetInt("_ZWrite", 0);
                standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                standardShaderMaterial.renderQueue = 3000;
                break;
        }

    }

}
