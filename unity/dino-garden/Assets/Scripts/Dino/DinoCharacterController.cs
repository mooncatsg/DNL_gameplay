using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using DG.Tweening;

public class DinoCharacterController : MonoBehaviour
{
    [SerializeField] TextAsset jsonFile;
    [SerializeField] SkinnedMeshRenderer bodySkinnedMesh;
    [SerializeField] SkinnedMeshRenderer faceSkinnedMesh;
    [SerializeField] SkinnedMeshRenderer defaultBackSkinnedMesh;
    [SerializeField] SkinnedMeshRenderer defaultTeethSkinnedMesh;
    [SerializeField] List<GameObject> wingNodeList;
    [SerializeField] List<GameObject> teethNodeList;
    [SerializeField] List<GameObject> backNodeList;
    [SerializeField] List<GameObject> hornNodeList;
    [SerializeField] GameObject glassNode10;
    [SerializeField] public bool randomDino;
    [SerializeField] public bool autoRotate;
    [SerializeField] public bool autoMoveAroundCage;
    [SerializeField] public bool autoMoveAroundCrop;
    public int partNumber = 10;
    public int bodyNumber = 11;

    //public void Start()
    //{
    //    DinoModel dino = JsonConvert.DeserializeObject<DinoModel>(jsonFile.text);
    //    Debug.LogError(JsonConvert.SerializeObject(dino.getTraits()));
    //    Debug.LogError(JsonConvert.SerializeObject(dino.getExpressingTraits()));
    //    Debug.LogError("rarirty " + dino.calculateRarity());
    //}
    List<Vector3> pathCage = new List<Vector3>();
    List<Vector3> pathCrop = new List<Vector3>();

    public void Start()
    {
        //this.dinoController = this.node.parent.getComponent(DinoController);
        //this.wbHeroController = this.node.parent.getComponent(WBHeroController);
        #if UNITY_EDITOR
        if (this.randomDino)
        {
            this.randomNewDino();
        }
        #endif
        
        if (autoMoveAroundCage)
        {
            float rd = UnityEngine.Random.Range(0f, 3f);
            pathCage.Add(new Vector3(1.1f, 0, 1.1f));
            pathCage.Add(new Vector3(-1.1f, 0, 1.1f));
            pathCage.Add(new Vector3(-1.1f, 0, -1.1f));
            pathCage.Add(new Vector3(1.1f, 0, -1.1f));
            pathCage.Add(new Vector3(1.1f, 0, 1.1f));
            this.ActionWaitForSeconds(rd, () => {
                this.transform.DOLocalPath(pathCage.ToArray(), 20f, PathType.Linear).SetLoops(-1);
            });
           // this.transform.DOLocalPath(pathCage.ToArray(), 20f, PathType.Linear).SetLoops(-1);
        }
        if(autoMoveAroundCrop)
        {
            this.ActionWaitForSeconds(2f, () => {
                autoMoveCrop();
            });
            
        }
        if (autoRotate)
        {
            transform.GetChild(0).DOLocalRotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);
        }
    }
    public void Update() 
    {
        if (autoMoveAroundCage)
        {
            List<Vector3> rotationCage = new List<Vector3>();
            rotationCage.Add(new Vector3(0, -90, 0));
            rotationCage.Add(new Vector3(0, -180, 0));
            rotationCage.Add(new Vector3(0, 90, 0));
            rotationCage.Add(new Vector3(0, 0, 0));
            rotationCage.Add(new Vector3(0, -90, 0));
            for (int i = 0; i < 5; i++)
            {
                Vector3 dinoTranform = transform.parent.position - pathCage[i] * 10f;
                Vector3 dinoPosition = this.transform.position;
                Vector3 dinoTranform1 = transform.parent.position - pathCage[i] * 10f + (new Vector3(.2f, 0f, 0f));
                Vector3 dinoTranform2 = transform.parent.position - pathCage[i] * 10f + (new Vector3(0f, 0f, .2f));
                Vector3 dinoTranform3 = transform.parent.position - pathCage[i] * 10f + (new Vector3(-.2f, 0f, 0f));
                Vector3 dinoTranform4 = transform.parent.position - pathCage[i] * 10f + (new Vector3(0f, 0f, -.2f));
                if (dinoTranform.ToString() == dinoPosition.ToString() || dinoTranform1.ToString() == dinoPosition.ToString() || dinoTranform2.ToString() == dinoPosition.ToString()
                    || dinoTranform3.ToString() == dinoPosition.ToString() || dinoTranform4.ToString() == dinoPosition.ToString())
                {
                    this.transform.DORotate(rotationCage[i], .5f, RotateMode.Fast);
                }
            }
        }
        if (autoMoveAroundCrop)
        {
            List<Vector3> rotationCrop = new List<Vector3>();
            rotationCrop.Add(new Vector3(0, 90, 0));
            rotationCrop.Add(new Vector3(0, 0, 0));
            rotationCrop.Add(new Vector3(0, -90, 0));
            rotationCrop.Add(new Vector3(0, 180, 0));
            rotationCrop.Add(new Vector3(0, 90, 0));
            this.ActionWaitForSeconds(2f, () => {
                for (int i = 0; i < 5; i++)
                {
                    Vector3 dinoPosition = this.transform.position;
                    Vector3 dinoTranform = pathCrop[i];
                    Vector3 dinoTranform1 = pathCrop[i] + (new Vector3(.2f, 0f, 0f));
                    Vector3 dinoTranform2 = pathCrop[i] + (new Vector3(0f, 0f, .2f));
                    Vector3 dinoTranform3 = pathCrop[i] + (new Vector3(-.2f, 0f, 0f));
                    Vector3 dinoTranform4 = pathCrop[i] + (new Vector3(0f, 0f, -.2f));
                    if (dinoTranform.ToString() == dinoPosition.ToString() || dinoTranform1.ToString() == dinoPosition.ToString() || dinoTranform2.ToString() == dinoPosition.ToString()
                    || dinoTranform3.ToString() == dinoPosition.ToString() || dinoTranform4.ToString() == dinoPosition.ToString())
                    {
                        this.transform.DORotate(rotationCrop[i], .5f, RotateMode.Fast);
                    }
                }
            });
            
        }
    }
   public void autoMoveCrop()
    {
        int totalCrops = (MapController.instance.totalCropInLand());
        Vector3 INIT_POS_FARM = new Vector3(8.3f, 0f, -19f);
        pathCrop.Add(new Vector3(INIT_POS_FARM.x + 7f, 0f, -12f));
        if (totalCrops <= 6)
        {
            pathCrop.Add(new Vector3(INIT_POS_FARM.x - (((totalCrops - 1) % 6f) * 10f + 7f), 0f, -12f));
            pathCrop.Add(new Vector3(INIT_POS_FARM.x - (((totalCrops - 1) % 6f) * 10f + 7f), 0f, -26f));
            pathCrop.Add(new Vector3(INIT_POS_FARM.x + 7f, 0f, -26f));
        }
        else if(totalCrops <= 12)
        {
            pathCrop.Add(new Vector3(INIT_POS_FARM.x - ((5f % 6f) * 10f + 7f), 0f, -12f));
            pathCrop.Add(new Vector3(INIT_POS_FARM.x - ((5f % 6f) * 10f + 7f), 0f, INIT_POS_FARM.z - 8f * 2));
            pathCrop.Add(new Vector3(INIT_POS_FARM.x + 7f, 0f, INIT_POS_FARM.z - 8f * 2));
        }
        pathCrop.Add(new Vector3(INIT_POS_FARM.x + 7f, 0f, -12f));
        this.transform.DOLocalPath(pathCrop.ToArray(), totalCrops * 9f, PathType.Linear).SetLoops(-1);
    }
    //INIT_POS_FARM.z - 8f * 2
    //void randomOldDino()
    //{
    //    var rarity = UnityEngine.Random.Range(1, 5);
    //    var dino_class = UnityEngine.Random.Range(1, 3);
    //    this.loadDataVer1(dino_class, rarity);
    //}

    void randomNewDino()
    {
        var newDino = new DinoExpressTraits();
        newDino.Texture = UnityEngine.Random.Range(0, this.bodyNumber);
        newDino.Wing = UnityEngine.Random.Range(0, this.partNumber);
        newDino.Eye = UnityEngine.Random.Range(0, this.partNumber);
        newDino.Teeth = UnityEngine.Random.Range(0, this.partNumber);
        newDino.Back = UnityEngine.Random.Range(0, this.partNumber);
        newDino.Horn = UnityEngine.Random.Range(0, this.partNumber);
        this.loadDinoData(newDino, 5, 1);
    }



    ////#region Dino version 1
    //void loadClass(int dino_class)
    //{
    //    this.loadDataVer1(dino_class, this.currentRare);
    //}

    //void loadRare(int dino_rarity)
    //{
    //    this.loadDataVer1(this.currentClass, dino_rarity);
    //}
    //int currentClass;
    //int currentRare;
    //void loadDataVer1ByGenes(string genes)
    //{
    //    int dino_class = int.Parse(genes.Substring(0, 2)) % 10;
    //    int dino_rarity = int.Parse(genes.Substring(2, 2)) % 10;
    //    this.loadDataVer1(dino_class, dino_rarity);
    //}

   // void loadDataVer1(int dino_class, int dino_rarity)
   // {
   //     this.currentClass = dino_class;
   //     this.currentRare = dino_rarity;
   //     this.clearMesh();
   //     this.bodySkinnedMesh.gameObject.SetActive(true);
   //     this.faceSkinnedMesh.gameObject.SetActive(true);
   //     this.defaultBackSkinnedMesh.gameObject.SetActive(true);
   //     this.defaultTeethSkinnedMesh.gameObject.SetActive(true);
   //     this.getDinoV1Mat(dino_class, dino_rarity, (dinoMat) => {
   //         this.bodySkinnedMesh.material = dinoMat;
   //         this.defaultBackSkinnedMesh.material = dinoMat;
   //         this.defaultTeethSkinnedMesh.material = dinoMat;
   //     });

   //     this.getDinoV1FaceMat(dino_class, dino_rarity, (faceMat) => {
   //         this.faceSkinnedMesh.material = faceMat;
   //     });
   // }
   // void getDinoV1Mat(int dino_class, int dino_rarity, Action<Material> callbacks) 
   // {
   //     callbacks(Resources.Load<Material>("MaterialDino/dino_"+dino_class+"_"+dino_rarity));
   // }

   //void getDinoV1FaceMat(int dino_class, int dino_rarity, Action<Material> callbacks)
   // {
   //     callbacks(Resources.Load<Material>("MaterialDino/dino_face_" + dino_class + "_" + dino_rarity));
   // }


    void loadBoby(int bodyId){
        this.getBodyMat(bodyId, (bodyMat) => {
            this.bodySkinnedMesh.material = bodyMat;
        });
    }

    void loadWing(int wingId, int rarity, int nftId){
        foreach (var element in wingNodeList)
        {
            element.SetActive(false);
        };
        if (nftId % 2 == 0 && wingId >= 4)
        {
            wingId -= 4;
        }
        if (rarity >= 4 && wingId >= 0 && wingId < this.partNumber)
        {

            this.getBodyMat(wingId, (wingMat) => {
                //this.wingNodeList[wingId].getComponent(SkinnedMeshRenderer).material = wingMat;
                foreach (var element in wingNodeList[wingId].GetComponents<SkinnedMeshRenderer>())
                {
                    element.material = wingMat;
                };
                
            });
            this.wingNodeList[wingId].SetActive(true);
        }
    }
    void loadFace(int faceId){
        this.getFaceMat(faceId, (faceMat) => {
            this.faceSkinnedMesh.material = faceMat;
        });
    }
    void loadTeeth(int teethId){
        foreach (var element in teethNodeList)
        {
            element.SetActive(false);
        };
       
        if (teethId >= 0 && teethId < this.partNumber)
        {
            this.getBodyMat(teethId, (teethMat) => {
                this.teethNodeList[teethId].GetComponent<SkinnedMeshRenderer>().material = teethMat;
            });
            this.teethNodeList[teethId].SetActive(true);
        }
    }
    void loadBack(int backId){
        
        foreach (var element in backNodeList)
        {
            element.SetActive(false);
        };
        if (backId >= 0 && backId < this.partNumber)
        {
            this.getBodyMat(backId, (backMat) => {
                this.backNodeList[backId].GetComponent<SkinnedMeshRenderer>().material = backMat;
            });
            this.backNodeList[backId].SetActive(true);
        }
    }
    void loadHorn(int hornId){
        foreach (var element in hornNodeList)
        {
            element.SetActive(false);
        };
       
        this.glassNode10.SetActive(false);

        if (hornId >= 0 && hornId < this.partNumber)
        {
            this.glassNode10.SetActive(hornId == 9);
            this.getBodyMat(hornId, (hornMat) => {
                this.hornNodeList[hornId].GetComponent<SkinnedMeshRenderer>().material = hornMat;
            });
            this.hornNodeList[hornId].SetActive(true);
        }
    }
    public void loadDinoData(DinoExpressTraits data, int rarity, int nftId){
        this.loadData(data.Texture, data.Eye, data.Wing, data.Teeth, data.Back, data.Horn, rarity, nftId);
    }
    void loadData(int bodyId, int faceId, int  wingId, int teethId, int backId, int hornId, int rarity, int nftId)
    {
        this.getFaceMat(faceId, (faceMat) => {
            this.faceSkinnedMesh.material = faceMat;
        });
        this.getBodyMat(bodyId, (bodyMat) => {
            this.bodySkinnedMesh.material = bodyMat;
        });

        this.loadWing(wingId, rarity, nftId);
        this.loadTeeth(teethId);
        this.loadBack(backId);
        this.loadHorn(hornId);
    }

    void getBodyMat(int id, Action<Material> callbacks)
    {
        callbacks(Resources.Load<Material>("DinoPart/Body/part_dino_body_" + (id + 1))); 
    }

    void getFaceMat(int id, Action<Material> callbacks)
    {
        callbacks(Resources.Load<Material>("DinoPart/Face/part_dino_face_" + (id + 1)));
    }

//#endregion    
    void clearMesh()
    {
        foreach (var element in wingNodeList)
        {
            element.SetActive(false);
        };
        foreach (var element in teethNodeList)
        {
            element.SetActive(false);
        };
        foreach (var element in backNodeList)
        {
            element.SetActive(false);
        };
        foreach (var element in hornNodeList)
        {
            element.SetActive(false);
        };
        this.defaultBackSkinnedMesh.gameObject.SetActive(false);
    }
}
