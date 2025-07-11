using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MarbleDataInput : MonoBehaviour
{
   [SerializeField]
   private GameManager gameManager;

   [Header("UI")]
   [Header("InputField")]
   [SerializeField]
   private TMP_InputField marbleNameInputField;
   [SerializeField]
   private TMP_InputField marbleCountInputField;
   
   [Header("Buttons")]
   [SerializeField]
   private Button addMarbleButton;
   [SerializeField]
   private Button removeMarbleButton;
   [SerializeField]
   private Button resetMarbleButton;
   private void Awake()
   {
      if (!gameManager)
      {
         gameManager = FindFirstObjectByType<GameManager>();
         Assert.IsNotNull(gameManager);
         if (!gameManager)
         {
            gameManager.AddComponent<GameManager>();
         }
      }
      
      gameManager.OnRacingStateChanged.AddListener(OnRacingStateChanged);
      
      addMarbleButton.onClick.AddListener(OnClickAddMarbleButton);
      removeMarbleButton.onClick.AddListener(OnClickRemoveMarbleButton);
      resetMarbleButton.onClick.AddListener(OnClickResetMarbleButton);
   }

   private void OnClickAddMarbleButton()
   {
      int marbleCount = int.Parse(marbleCountInputField.text);
      gameManager.AddMarble(marbleNameInputField.text, marbleCount);
   }

   private void OnClickRemoveMarbleButton()
   {
      int marbleCount = int.Parse(marbleCountInputField.text);
      gameManager.RemoveMarble(marbleNameInputField.text, marbleCount);
   }

   private void OnClickResetMarbleButton()
   {
      
   }

   private void OnRacingStateChanged(bool isRacing)
   {
      addMarbleButton.interactable = !isRacing;
      removeMarbleButton.interactable = !isRacing;
      resetMarbleButton.interactable = !isRacing;
   }
}
