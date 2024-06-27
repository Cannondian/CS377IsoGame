using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using RPGCharacterAnims;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ModifierChoice : Singleton<ModifierChoice>
{
    public int nextLevel;
    public GameObject choiceBoxes;
    public GameObject choiceBox1;
    public GameObject choiceBox2;
    public GameObject choiceBox3;

    [SerializeField] private GameObject choice1BG;
    [SerializeField] private GameObject choice2BG;
    [SerializeField] private GameObject choice3BG;

    private List<GameObject> backgrounds;
    
    
    
    [FormerlySerializedAs("ILModText")] [SerializeField] private TMP_ColorGradient ILModGradient;
    [FormerlySerializedAs("VHModText")] [SerializeField] private TMP_ColorGradient VHModGradient;
    [FormerlySerializedAs("SHModText")] [SerializeField] private TMP_ColorGradient SHModGradient;
    
    [SerializeField] private Material TSMMat;
    [SerializeField] private Material TEEMat;
    [SerializeField] private Material REMMat;
    
    
    
    public List<GameObject> boxList;
    public RPGCharacterController controller;
    public bool displayingBoxes;
    private List<ModifierVault.IModifier> modifiersBeingDisplayed;
    public UnityEvent makeModifierChoiceInputAvailable;
    public UnityEvent modifierApplied;
    
    
    // Start is called before the first frame update
    void Start()
    {
        boxList.Add(choiceBox1);
        boxList.Add(choiceBox2);
        boxList.Add(choiceBox3);
        backgrounds = new List<GameObject> { choice1BG, choice2BG, choice3BG };
    }
    public void ToggleChoiceBoxes(int level)
    {
        choiceBoxes.SetActive(true);
        displayingBoxes = true;
        makeModifierChoiceInputAvailable.Invoke();
        
        
        controller.Lock(true, true, false, 0, -1);

        modifiersBeingDisplayed = 
            ModifierVault.GenerateModifiers(new List<ModifierVault.IModifier>());
        for (int i = 0; i < 3; i++)
        {
            
            var boxText = boxList[i].GetComponentInChildren<TextMeshProUGUI>();
            boxText.enableVertexGradient = false;
            Debug.Log("box text assign");
            if (modifiersBeingDisplayed[i] is ModifierVault.TileStatModifier)
            {
                
                
                
                var backgroundImage = backgrounds[i].GetComponent<UnityEngine.UI.Image>();
                backgroundImage.material = TSMMat;
                
                string elementWord = ((ModifierVault.TileStatModifier)modifiersBeingDisplayed[i]).myStat.ToString();
                TMP_ColorGradient elementGradient = ILModGradient; //default type;
                switch (((ModifierVault.TileStatModifier)modifiersBeingDisplayed[i]).myStat)
                {
                    case TileElement.ElementType.Shalharan:
                        elementGradient = SHModGradient;
                        break;
                    case TileElement.ElementType.Velheret:
                        elementGradient = VHModGradient;
                        break;
                }
               
                string modText = "Increase your mastery over " + 
                               elementWord +
                               " by " +  (((ModifierVault.TileStatModifier)modifiersBeingDisplayed[i]).myRarity * 10).ToString();
                string gradientTag = $"<gradient=\"{elementGradient.name}\">{elementWord}</gradient>";
                boxText.text =  modText.Replace(elementWord, gradientTag);
            }

            if (modifiersBeingDisplayed[i] is ModifierVault.TileEffectEnhancement)
            {
                var currentModifier = (ModifierVault.TileEffectEnhancement)modifiersBeingDisplayed[i];
                var backgroundImage = backgrounds[i].GetComponent<UnityEngine.UI.Image>();
                if (backgroundImage == null)
                {
                    print("used the wrong image component!!");
                }

                backgroundImage.material = TEEMat;
                switch (currentModifier.element)
                {
                    
                    case TileElement.ElementType.Ilsihre:
                        boxText.text = ModifierText.TEEList["Ilsihre" + currentModifier.modIndex.ToString()];
                        boxText.enableVertexGradient = true;
                        boxText.colorGradientPreset = ILModGradient;
                        break;
                    case TileElement.ElementType.Shalharan:
                        boxText.text = ModifierText.TEEList["Shalharan" + currentModifier.modIndex.ToString()];
                        boxText.enableVertexGradient = true;
                        boxText.colorGradientPreset = SHModGradient;
                        break;
                    case TileElement.ElementType.Velheret:
                        boxText.text = ModifierText.TEEList["Velheret" + currentModifier.modIndex.ToString()];
                        boxText.enableVertexGradient = true;
                        boxText.colorGradientPreset = VHModGradient;
                        break;
                }
               
            }
        }
    }

    public void MakeModifierChoice(int choice)
    {
        if (displayingBoxes)
        {
            ModifierVault.IModifier chosenModifier = modifiersBeingDisplayed[choice-1];
            chosenModifier.ApplyModifier();
            if (chosenModifier is ModifierVault.TileEffectEnhancement)
            {
                TileMastery.Instance.appliedTEE.Add(chosenModifier);
                print("we have added to the list of TEE");
            }
                
            
            HighlightChoice(choice);
            StartCoroutine(TransitionSequence());
        }
    }

    private IEnumerator TransitionSequence()
    {

        yield return new WaitForSeconds(4);
        displayingBoxes = false;
        controller.Unlock(true, true);
        modifierApplied.Invoke();
    }

    public void HighlightChoice(int choice)
    {
        Animator animator = boxList[choice - 1].GetComponent<Animator>();
        animator.SetBool("Flashing", true);
    }
    
    public void LoadNextLevel()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
