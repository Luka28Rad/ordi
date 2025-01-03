using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressiveFillWithContinuousAnimations : MonoBehaviour
{
    public GameObject[] objects; // Assign your GameObjects in the inspector
    public GameObject[] animationTargets; // Assign the corresponding objects to animate
    public TextMeshProUGUI[] tmpTexts; // Assign the corresponding TMP texts to reveal
    public float fillSpeed = 0.5f; // Speed at which the images are filled
    public float scaleSpeed = 1f; // Speed for scaling animation
    public float rotationSpeed = 30f; // Speed for rotation animation
    public float textRevealSpeed = 0.1f; // Speed for revealing text characters
    public GameObject winnerChar;

    private int currentIndex;

    void OnEnable()
    {
        // Start with the last object
        currentIndex = objects.Length - 1;

        // Ensure all objects start with fill amount 0, children are invisible, and texts are hidden
        foreach (var obj in objects)
        {
            Image img = obj.GetComponent<Image>();
            if (img != null)
                img.fillAmount = 0;

            SetChildrenActive(obj, false);
        }

        foreach (var tmpText in tmpTexts)
        {
            if (tmpText != null)
                tmpText.text = ""; // Clear the text initially
        }

        // Start the filling process
        StartCoroutine(FillObjects());
        StartCoroutine(SpinWinner());
    }

    private IEnumerator SpinWinner(){
        while (true)
        {
            winnerChar.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }

    private IEnumerator FillObjects()
    {
        while (currentIndex >= 0)
        {
            GameObject currentObject = objects[currentIndex];
            GameObject animationTarget = animationTargets[currentIndex];
            TextMeshProUGUI currentText = tmpTexts[currentIndex];
            Image img = currentObject.GetComponent<Image>();

            if (img != null)
            {
                // Gradually fill the image
                while (img.fillAmount < 1)
                {
                    img.fillAmount += Time.deltaTime * fillSpeed;
                    yield return null;
                }

                // When filled, make children visible and start animation on the target
                SetChildrenActive(currentObject, true);

                if (animationTarget != null)
                {
                    animationTarget.SetActive(true);
                    StartCoroutine(StartRandomAnimation(animationTarget));
                }

                // Start revealing text if present
                if (currentText != null)
                {
                    StartCoroutine(RevealText(currentText));
                }
            }

            // Move to the next object (2nd to last, and so on)
            currentIndex--;
        }
        yield return new WaitForSeconds(1f);
        FindObjectOfType<CustomNetworkManager>().SendReturnToLobby();
    }

    private IEnumerator RevealText(TextMeshProUGUI tmpText)
    {
        if(tmpText.text == "") tmpText.text = "---";
        string fullText = tmpText.text;
        tmpText.text = ""; // Start with an empty text

        for (int i = 0; i < fullText.Length; i++)
        {
            tmpText.text += fullText[i];
            yield return new WaitForSeconds(textRevealSpeed);
        }
    }

    private IEnumerator StartRandomAnimation(GameObject target)
    {
        int randomAnimation = Random.Range(0, 3); // Choose between 0, 1, and 2

        switch (randomAnimation)
        {
            case 0: // Rotate along Y-axis
                StartCoroutine(RotateYContinuous(target));
                break;
            case 1: // Rotate in Z-axis
                StartCoroutine(RotateZContinuous(target));
                break;
            case 2: // Scale continuously
                StartCoroutine(ScaleContinuous(target));
                break;
        }

        yield break; // No need to stop since animations are continuous
    }

    private IEnumerator RotateYContinuous(GameObject target)
    {
        while (true)
        {
            target.transform.Rotate(0, rotationSpeed * 1.5f * Time.deltaTime, 0);
            yield return null;
        }
    }

    private IEnumerator RotateZContinuous(GameObject target)
    {
        float currentAngle = target.transform.rotation.eulerAngles.z;
        float targetAngle = currentAngle + 30f;
        float maxAngle = 30f;
        float minAngle = -30f;
        bool goingUp = true;

        while (true)
        {
            float step = rotationSpeed * Time.deltaTime;
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, step);

            target.transform.rotation = Quaternion.Euler(target.transform.rotation.eulerAngles.x, target.transform.rotation.eulerAngles.y, currentAngle);

            if (Mathf.Approximately(currentAngle, targetAngle))
            {
                goingUp = !goingUp;
                targetAngle = goingUp ? maxAngle : minAngle;
            }

            yield return null;
        }
    }

    private IEnumerator ScaleContinuous(GameObject target)
    {
        Vector3 originalScale = target.transform.localScale;
        Vector3 maxScale = originalScale * 1.5f;
        Vector3 minScale = originalScale * 0.5f;
        Vector3 targetScale = maxScale;
        bool scalingUp = true;

        while (true)
        {
            target.transform.localScale = Vector3.MoveTowards(target.transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

            if (Vector3.Distance(target.transform.localScale, targetScale) < 0.01f)
            {
                scalingUp = !scalingUp;
                targetScale = scalingUp ? maxScale : minScale;
            }

            yield return null;
        }
    }

    private void SetChildrenActive(GameObject obj, bool isActive)
    {
        foreach (Transform child in obj.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}
