using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager instance;

    public Transform camera;
    public Placeables placeables;
    public Material preview_valid;
    public Material preview_invalid;
    public bool enabled = false;
    public LayerMask layers;
    public GameObject buildMenu;

    private int selectedId = 0;
    private GameObject previewObject;
    private ColliderBridge previewCollider;
    bool valid = true;
    float rotation = 0;
    bool initBuildMenu = false;

    void Start()
    {
        instance = this;
    }

    public void ActivateBuildMode()
    {
        enabled = true;
        ToggleBuildMenu(true);
    }

    public void DeactivateBuildMode()
    {
        enabled = false;
        ToggleBuildMenu(false);
    }

    public void CheckIfPlacementIsValid()
    {
        
    }

    private void RemovePreview()
    {
        Destroy(previewObject);
        previewObject = null;
    }

    public void Place()
    {
        if (previewCollider.collisionCount == 0)
        {
            Vector3 pos = previewObject.transform.position - placeables.buildings[selectedId].pre_pos_offset + placeables.buildings[selectedId].pos_offset;
            RemovePreview();
            var obj = Instantiate(placeables.buildings[selectedId].prefab, pos, Quaternion.Euler(placeables.buildings[selectedId].rot_offset + new Vector3(0, rotation, 0)));
        }
    }

    public void ToggleBuildMenu(bool en)
    {
        if (!initBuildMenu)
        {

        }
        PlayerMovement.instance.Pause(en);
        buildMenu.SetActive(en);
    }
    
    public void SelectBuildMenuItem(int id)
    {
        Debug.Log(id);
        selectedId = id;
        buildMenu.SetActive(!buildMenu.activeSelf);
        PlayerMovement.instance.Pause(false);
        RemovePreview();
    }

    void Update()
    {
        if (enabled)
        {
            if (Input.GetButtonDown("BuildMenu"))
            {
                ToggleBuildMenu(!buildMenu.activeSelf);
            }

            rotation += Input.GetAxis("Mouse ScrollWheel") * 30;
            RaycastHit hit;
            if (Physics.Raycast(camera.position, camera.TransformDirection(Vector3.forward), out hit, 100, layers))
            {
                Vector3 pos = hit.point + placeables.buildings[selectedId].pre_pos_offset;

                if (previewObject == null)
                {
                    previewObject = Instantiate(placeables.buildings[selectedId].preview, pos, Quaternion.Euler(placeables.buildings[selectedId].pre_rot_offset + new Vector3(0, rotation, 0)));
                    previewCollider = previewObject.AddComponent<ColliderBridge>();
                    valid = previewCollider.collisionCount == 0;
                    previewObject.GetComponent<MeshRenderer>().material = valid ? preview_valid : preview_invalid;
                }
                else
                {
                    bool validBefore = valid;
                    valid = previewCollider.collisionCount == 0;
                    if (valid != validBefore)
                    {
                        previewObject.GetComponent<MeshRenderer>().material = valid ? preview_valid : preview_invalid;
                    }
                    previewObject.transform.position = pos;
                    previewObject.transform.rotation = Quaternion.Euler(placeables.buildings[selectedId].pre_rot_offset + new Vector3(0, rotation, 0));
                }
            }
            else if (previewObject != null)
            {
                RemovePreview();
            }
        }
        else
        {
            if (previewObject != null)
            {
                RemovePreview();
            }
        }
    }
}
