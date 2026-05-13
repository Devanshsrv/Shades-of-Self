using UnityEngine;

public class SimpleNPCDialogue : MonoBehaviour
{
    public enum NPCType { Push, Collect, Choice }

    [Header("Setup")]
    public NPCType npcType;
    public string npcName = "NPC";

    [Header("Push / Collect NPC")]
    [TextArea] public string beforeTaskLine;
    [TextArea] public string afterTaskLine;

    [Header("Choice NPC")]
    [TextArea] public string questionLine;
    public string[] options;          // up to 3 options
    public int correctOptionIndex = 0; // 0,1,2 (for option 1/2/3)

    private bool playerInside = false;
    private bool taskDone = false;
    private bool choiceActive = false;
    private bool itemCollected = false;

    void Update()
    {
        if (!playerInside) return;

        // Talk / open dialogue with E
        if (!choiceActive && Input.GetKeyDown(KeyCode.E))
        {
            HandleInteract();
        }

        // Handle answers for Choice NPC (keys 1/2/3)
        if (choiceActive)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) HandleChoice(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) HandleChoice(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) HandleChoice(2);
        }
    }

    void HandleInteract()
    {
        var ui = SimpleDialogueUI.Instance;
        if (ui == null) return;

        if (npcType == NPCType.Choice)
        {
            if (taskDone)
            {
                // Already solved → no more choices
                ui.ShowLine(npcName, "Thank you!");
                ui.ShowChoices(null);
                return;
            }

            // First time → show question and options
            ui.ShowLine(npcName, questionLine);
            ui.ShowChoices(options);
            choiceActive = true;
        }

        else if (npcType == NPCType.Collect)
        {
            // COLLECT NPC LOGIC
            if (!taskDone)
            {
                if (!itemCollected)
                {
                    // Player hasn't picked the coin yet
                    ui.ShowLine(npcName, beforeTaskLine);   // "I lost my coin..."
                }
                else
                {
                    // Coin already collected, now finishing quest
                    MarkTaskDone();                          // <-- shrink wisp HERE
                    ui.ShowLine(npcName, afterTaskLine);     // "You found it!"
                }
            }
            else
            {
                // Quest already completed, always show after line
                ui.ShowLine(npcName, afterTaskLine);
            }

            ui.ShowChoices(null);
        }
        else // NPCType.Push
        {
            // PUSH NPC behavior remains same
            string line = taskDone ? afterTaskLine : beforeTaskLine;
            ui.ShowLine(npcName, line);
            ui.ShowChoices(null);
        }
    }


    void HandleChoice(int choiceIndex)
    {
        if (taskDone) return; 
        var ui = SimpleDialogueUI.Instance;
        if (ui == null) return;

        choiceActive = false;

        bool correct = (choiceIndex == correctOptionIndex);

        string response = correct
            ? "Thank you... I feel a little better."
            : "That doesn't really help...";

        ui.ShowLine(npcName, response);
        ui.ShowChoices(null);

        if (correct)
        {
            taskDone = true;
            FindObjectOfType<GuiltWispController>()?.ShrinkOnce();
            // if you want to hook wisp shrinking etc later, this is the spot
        }
    }

    // For push/collect NPC: call this from your puzzle/collect scripts when done
    public void MarkTaskDone()
    {
        taskDone = true;
        FindObjectOfType<GuiltWispController>()?.ShrinkOnce();
    }

    public void MarkCollected()
    {
        if (npcType == NPCType.Collect)
            itemCollected = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER ENTERED NPC TRIGGER");
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            choiceActive = false;
            Debug.Log("PLAYER EXITED NPC TRIGGER");

            var ui = SimpleDialogueUI.Instance;
            if (ui != null && ui.IsOpen)
                ui.HideAll();
        }
    }
}
