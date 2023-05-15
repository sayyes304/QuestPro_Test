using UnityEngine;
using Meta.WitAi;
using UnityEngine.UI;
using TMPro;
using Meta.WitAi.Json;


namespace Oculus.Voice { 
public class Voice_Handler : MonoBehaviour
{
    [SerializeField] private string DefaultText = "  �Է��ϼ���. ";

    public TextMeshProUGUI BtnText;

    public GameObject KeyboardTxt;
    public GameObject VoiceTxt;



        [Header("UI")]
    [SerializeField] private TextMeshProUGUI textArea;
    [SerializeField] private bool showJson;

    [Header("Voice")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;

    // Whether voice is activated
    public bool IsActive => _active;
    private bool _active = false;


    private void OnEnable()
    {
        textArea.text = DefaultText;
        appVoiceExperience.VoiceEvents.OnRequestCreated.AddListener(OnRequestStarted);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnStartListening.AddListener(OnListenStart);
        appVoiceExperience.VoiceEvents.OnStoppedListening.AddListener(OnListenStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToDeactivation.AddListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToInactivity.AddListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnRequestResponse);
        appVoiceExperience.VoiceEvents.OnError.AddListener(OnRequestError);
    }
    // Remove delegates
    private void OnDisable()
    {
        appVoiceExperience.VoiceEvents.OnRequestCreated.RemoveListener(OnRequestStarted);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnRequestTranscript);
        appVoiceExperience.VoiceEvents.OnStartListening.RemoveListener(OnListenStart);
        appVoiceExperience.VoiceEvents.OnStoppedListening.RemoveListener(OnListenStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToDeactivation.RemoveListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnStoppedListeningDueToInactivity.RemoveListener(OnListenForcedStop);
        appVoiceExperience.VoiceEvents.OnResponse.RemoveListener(OnRequestResponse);
        appVoiceExperience.VoiceEvents.OnError.RemoveListener(OnRequestError);
    }

    // Request began
    private void OnRequestStarted(WitRequest r)
    {
        // Store json on completion
        if (showJson) r.onRawResponse = (response) => textArea.text = response;
        // Begin
        _active = true;
    }


    // �ǽð����� ���� �Է� �� response
    private void OnRequestTranscript(string transcript)
    {
        textArea.text = transcript;
    }


    // Listen start
    private void OnListenStart()
    {
        textArea.text = " ��� ��...";
    }
    // Listen stop
    private void OnListenStop()
    {
        textArea.text = "Processing...";
    }

    // activate�� ���� �ʰ� 
    private void OnListenForcedStop()
    {
        if (!showJson)
        {
            textArea.text = DefaultText;
        }
        OnRequestComplete();
    }

    // Request response
    private void OnRequestResponse(WitResponseNode response)
    {
        if (!showJson)
        {
            if (!string.IsNullOrEmpty(response["text"]))
            {
                textArea.text = response["text"];
                    BtnText.text = "  ���� �Է�";
            }
            else
            {
                textArea.text = DefaultText;
            }
        }
        OnRequestComplete();
    }
    // Request error
    private void OnRequestError(string error, string message)
    {
        if (!showJson)
        {
            textArea.text = $"<color=\"red\">Error: {error}\n\n{message}</color>";
        }
        OnRequestComplete();
    }
    // Deactivate
    private void OnRequestComplete()
    {
        _active = false;
    }

    // Toggle activation
    public void ToggleActivation()
    {
            KeyboardTxt.SetActive(false);
            VoiceTxt.SetActive(true);

        SetActivation(!_active);

    }
        // Set activation
        public void SetActivation(bool toActivated)
        {
            if (_active != toActivated)
            {
                _active = toActivated;
                if (_active)
                {
                    appVoiceExperience.Activate();
                    BtnText.text = "  ������ ...";
                }
                else
                {
                    appVoiceExperience.Deactivate();
                }
            }
        }
    }
}
