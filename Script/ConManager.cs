using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ConManager : MonoBehaviour
{
    //-------------------------------------------------------------
    //変数設定
    private GameObject[] ParticleExamples;
    private GameObject particle;
    private int exampleIndex = 0;
    private bool slowMo;
    [SerializeField]
    private float p_high = 0.0f;
    [SerializeField]
    private float repeat = 0.0f;
    private bool flag = false;
    private List<GameObject> onScreenParticles = new List<GameObject>();

    private float timer = 0.0f;
    //----------------------------------------------
    //起動時
    void Awake()
    {
        List<GameObject> particleExampleList = new List<GameObject>();
        int nbChild = this.transform.childCount;
        for(int i = 0; i < nbChild; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            particleExampleList.Add(child);
        }
/*        particleExampleList.Sort( delegate(GameObject o1, GameObject o2) { return o1.name.CompareTo(o2.name); } );*/
        ParticleExamples = particleExampleList.ToArray();

        
        StartCoroutine("CheckForDeletedParticles");
    }
    
    //繰り返し
    void Update()
    {
        //particle.transform.position = this.transform.parent.position;
        //particle.transform.rotation = this.transform.parent.rotation;
    }
    //-------------------------------------------------------------

    
    //---------------------------------------------
    // SYSTEM

    //エフェクト生成
    public void Generate(int cmd)
    {
        //GameObject particle;
        flag = true;
        switch (cmd)
        {
            case 1:
                Debug.Log("ConSmaller");
                SetExampleIndex(1);
                particle = spawnParticle();
                timer = 0.0f;
                break;
            case 2:
                Debug.Log("ConSmall");
                SetExampleIndex(2);
                particle = spawnParticle();
                timer = 0.0f;
                break;
            case 3:
                Debug.Log("ConBig");
                SetExampleIndex(3);
                particle = spawnParticle();
                timer = 0.0f;
                break;
            case 4:
                Debug.Log("ConBigger");
                SetExampleIndex(4);
                particle = spawnParticle();
                timer = 0.0f;
                break;
            
        }
    }
    //表示するエフェクトの指定
    private void SetExampleIndex(int cmd)
    {
        exampleIndex = cmd-1;
    }
    //エフェクト表示
    private GameObject spawnParticle()
    {
        GameObject particles = (GameObject)Instantiate(ParticleExamples[exampleIndex]);
        particles.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
        #if UNITY_3_5
            particles.SetActiveRecursively(true);
        #else
            particles.SetActive(true);
//            for(int i = 0; i < particles.transform.childCount; i++)
//                particles.transform.GetChild(i).gameObject.SetActive(true);
        #endif
        
        ParticleSystem ps = particles.GetComponent<ParticleSystem>();

#if UNITY_5_5_OR_NEWER
        if (ps != null)
        {
            var main = ps.main;
            if (main.loop)
            {
                ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
                ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
            }
        }
#else
        if(ps != null && ps.loop)
        {
            ps.gameObject.AddComponent<CFX_AutoStopLoopedEffect>();
            ps.gameObject.AddComponent<CFX_AutoDestructShuriken>();
        }
#endif

        onScreenParticles.Add(particles);
        
        return particles;
    }
    
    IEnumerator CheckForDeletedParticles()
    {
        while(true)
        {
            yield return new WaitForSeconds(5.0f);
            for(int i = onScreenParticles.Count - 1; i >= 0; i--)
            {
                if(onScreenParticles[i] == null)
                {
                    onScreenParticles.RemoveAt(i);
                }
            }
        }
    }
    
}
