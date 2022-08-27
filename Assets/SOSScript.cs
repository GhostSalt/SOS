using SOS;
using KModkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class SOSScript : MonoBehaviour
{
    static int _moduleIdCounter = 1;
    int _moduleID = 0;

    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public GameObject Aerial, SignalParent;
    public SpriteRenderer SignalSprite;
    public KMSelectable[] Buttons, Switches;
    public MeshRenderer[] Bulbs;
    public TextMesh[] Texts;

    private KMAudio.KMAudioRef Sound;

    private Coroutine[] Anims;
    private Coroutine StrikeAnim;
    private Coroutine BlinkAnim;
    private List<int> FlightPathNums = new List<int>();
    private int Address, CurrentTime, PlanePos;
    private float StartPos;
    private string Input, Message;
    private bool[,] BinaryNumbers = new bool[2, 8];
    private bool[] SwitchPositions = new bool[5];
    private bool Active, Blinking, Solved;
    private List<Color> RandomColours = new List<Color>();

    private string[] PlaneModels =
    {
        "CRYYWYY",
        "BYMRBBRYRK",
        "CRM MWRRYR",
        "BRBY",
        "KRYMMYRYW",
        "BWB BMB",
        "RBBR WYYCK",
        "CRCK RMWYK"
    };

    void Awake()
    {
        _moduleID = _moduleIdCounter++;
        Texts[0].text = "Hello, player! GhostSalt\nhere. If you're reading\nthis message, the module\nis broken. Please tell me\r\non Discord\n(GhostSalt#0217) and I'll\nfix the issue.\nThanks!";
        Texts[1].text = "ATTENTION:\nText undefined.\nThis isn't good.";
        Anims = new Coroutine[Buttons.Length + Switches.Length];
        StartPos = Buttons[0].transform.localPosition.y;
        for (int i = 0; i < 16; i++)
            RandomColours.Add(Rnd.ColorHSV(30f / 360, 40f / 360, .85f, .85f, .9f, 1f));
        Module.OnActivate += delegate { Active = true; BlinkAnim = StartCoroutine(LightBulbs()); StartCoroutine(CheckPlanes()); };
        SignalParent.transform.localScale = new Vector3();
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = i;
            Buttons[x].OnInteract += delegate { if (Active) ButtonPress(x); return false; };
        }
        for (int i = 0; i < Switches.Length; i++)
        {
            int x = i;
            Switches[x].OnInteract += delegate { if (Active) SwitchToggle(x); return false; };
        }
        Calculate();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Calculate()
    {
        var possiblePaths = Enumerable.Range(0, 4 * PlaneModels.Length).ToList();
        possiblePaths.Shuffle();
        for (int i = 0; i < 8; i++)
            FlightPathNums.Add(possiblePaths[i]);
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 8; j += 2)
            {
                BinaryNumbers[i, j] = FlightPathNums[(i * 4) + (j / 2)] % 2 == 1;
                BinaryNumbers[i, j + 1] = FlightPathNums[(i * 4) + (j / 2)] % 4 >= 2;
            }
        Debug.Log(FlightPathNums.Join(", "));

        Texts[0].text = FlightPathNums.Select(x => PlaneModels[x / 4]).Join("\n");
    }

    void ButtonPress(int pos)
    {
        try
        {
            StopCoroutine(Anims[pos]);
        }
        catch { }
        Anims[pos] = StartCoroutine(ButtonAnim(pos));
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, Buttons[pos].transform);
        if (!Solved)
        {
            StartCoroutine(Signal());
            for (int i = 0; i < 8; i++)
                if (Address == Data.FlightPaths[FlightPathNums[i]][PlanePos])
                {
                    try
                    {
                        StopCoroutine(StrikeAnim);
                    }
                    catch { }
                    StrikeAnim = StartCoroutine(Strike());
                    goto end;
                }
            if (Blinking != (pos == 1))
            {
                if (Blinking)
                {
                    Blinking = false;
                    try
                    {
                        StopCoroutine(BlinkAnim);
                    }
                    catch { }
                    for (int i = 0; i < 16; i++)
                        Bulbs[i].material.color = new Color();

                }
                else
                    BlinkAnim = StartCoroutine(LightBulbs());
            }
            if (pos == 0)
            {
                if (SwitchPositions != new[] { true, true, true, true, true })
                    Input += Data.Alphabet[Data.BinToDec(SwitchPositions)];
                else
                {
                    if ()
                }
                Debug.Log(Input);
            }
            else
                Input = "";
            //StartCoroutine(Solve());
        }
        end:;
    }

    void SwitchToggle(int pos)
    {
        try
        {
            StopCoroutine(Anims[pos + Buttons.Length]);
        }
        catch { }
        Anims[pos + Buttons.Length] = StartCoroutine(SwitchAnim(pos));
        Audio.PlaySoundAtTransform("switch", Switches[pos].transform);
        SwitchPositions[pos] = !SwitchPositions[pos];
    }

    private IEnumerator CheckPlanes()
    {
        while (!Solved)
        {
            if (Mathf.FloorToInt(Bomb.GetTime()) != CurrentTime)
            {
                CurrentTime = Mathf.FloorToInt(Bomb.GetTime());
                PlanePos = (PlanePos + 1) % 16;
                Debug.Log(PlanePos);
                Debug.Log(FlightPathNums.Select(x => Data.FlightPaths[x][PlanePos]).Join(", "));
            }
            yield return null;
        }
    }

    private IEnumerator ButtonAnim(int pos, float duration = 0.05f, float depression = 0.003f)
    {
        Buttons[pos].AddInteractionPunch(0.5f);
        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            Buttons[pos].transform.localPosition = Vector3.Lerp(new Vector3(Buttons[pos].transform.localPosition.x, StartPos, Buttons[pos].transform.localPosition.z),
                new Vector3(Buttons[pos].transform.localPosition.x, StartPos - depression, Buttons[pos].transform.localPosition.z), timer / duration);
        }
        Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, StartPos - depression, Buttons[pos].transform.localPosition.z);
        timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            Buttons[pos].transform.localPosition = Vector3.Lerp(new Vector3(Buttons[pos].transform.localPosition.x, StartPos - depression, Buttons[pos].transform.localPosition.z),
                new Vector3(Buttons[pos].transform.localPosition.x, StartPos, Buttons[pos].transform.localPosition.z), timer / duration);
        }
        Buttons[pos].transform.localPosition = new Vector3(Buttons[pos].transform.localPosition.x, StartPos, Buttons[pos].transform.localPosition.z);
    }

    private IEnumerator SwitchAnim(int pos, float duration = 0.075f)
    {
        var baseObj = Switches[pos].GetComponentsInChildren<MeshRenderer>().Where(x => x.name == "Base").First();
        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            baseObj.transform.localEulerAngles = Vector3.Lerp(new Vector3(45 - (SwitchPositions[pos] ? 0 : 90), baseObj.transform.localEulerAngles.y, baseObj.transform.localEulerAngles.z),
                new Vector3(45 - (!SwitchPositions[pos] ? 0 : 90), baseObj.transform.localEulerAngles.y, baseObj.transform.localEulerAngles.z), timer / duration);
        }
        baseObj.transform.localEulerAngles = new Vector3(45 - (!SwitchPositions[pos] ? 0 : 90), baseObj.transform.localEulerAngles.y, baseObj.transform.localEulerAngles.z);
    }

    private IEnumerator Signal(float duration = 0.5f)
    {
        Audio.PlaySoundAtTransform("bleep", SignalParent.transform);
        var signal = Instantiate(SignalParent, SignalParent.transform.parent);
        signal.transform.localScale = new Vector3(0.35f, 0.35f, 1);
        signal.transform.localPosition = new Vector3();
        signal.transform.localEulerAngles = new Vector3(90, 0, 0);
        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            signal.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Lerp(0.75f, 0, timer / duration));
            signal.transform.localScale = Vector3.Lerp(new Vector3(0.35f, 0.35f, 1), new Vector3(3, 3, 1), timer / duration);
        }
        Destroy(signal);
    }

    private IEnumerator LightBulbs(float interval = 0.05f)
    {
        Blinking = true;
        int Chance = 5;
        while (true)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 8; j++)
                    Bulbs[(i * 8) + j].material.color = Rnd.Range(0, Chance) == 0 ? BinaryNumbers[i, j] ? RandomColours[(i * 8) + j] : new Color() : !BinaryNumbers[i, j] ? RandomColours[(i * 8) + j] : new Color();
            float timer = 0;
            while (timer < interval)
            {
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }

    private IEnumerator Strike(float duration = 1f)
    {
        Module.HandleStrike();
        try
        {
            Sound.StopSound();
        }
        catch { }
        Sound = Audio.PlaySoundAtTransformWithRef("strike", transform);
        Aerial.GetComponentsInChildren<MeshRenderer>().Where(x => x.name == "Ball").First().material.color = new Color(1, 0, 0);
        float timer = 0;
        while (timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        Aerial.GetComponentsInChildren<MeshRenderer>().Where(x => x.name == "Ball").First().material.color = new Color();
    }

    private IEnumerator Solve(float duration1 = .25f, float duration2 = 1.5f, float depression1 = .0025f, float depression2 = .05f, float pause = .05f)
    {
        Blinking = false;
        try
        {
            StopCoroutine(BlinkAnim);
        }
        catch { }
        StartCoroutine(UnlightBulbs());
        Solved = true;
        Audio.PlaySoundAtTransform("retract aerial", Aerial.transform);
        Vector3 start = Aerial.transform.localPosition;
        float timer = 0;
        while (timer < duration1)
        {
            yield return null;
            timer += Time.deltaTime;
            if (timer < duration1 - pause)
                Aerial.transform.localPosition = Vector3.Lerp(start, new Vector3(start.x, start.y - depression1, start.z), timer / (duration1 - pause));
            else
                Aerial.transform.localPosition = new Vector3(start.x, start.y - depression1, start.z);
        }
        timer = 0;
        while (timer < duration2)
        {
            yield return null;
            timer += Time.deltaTime;
            Aerial.transform.localPosition = Vector3.Lerp(new Vector3(start.x, start.y - depression1, start.z), new Vector3(start.x, start.y - depression1 - depression2, start.z), timer / duration2);
        }
        Aerial.transform.localPosition = new Vector3(start.x, start.y - depression1 - depression2, start.z);
        Audio.PlaySoundAtTransform("solve", transform);
        Module.HandlePass();
        Aerial.GetComponentsInChildren<MeshRenderer>().Where(x => x.name == "Ball").First().material.color = new Color(0, 1, 0);
    }

    private IEnumerator UnlightBulbs(float interval = 0.05f)
    {
        var litBulbs = new List<int>();
        for (int i = 0; i < 16; i++)
            if (Bulbs[i].material.color != new Color())
                litBulbs.Add(i);
        litBulbs.Shuffle();
        while (litBulbs.Count > 0)
        {
            Bulbs[litBulbs.First()].material.color = new Color();
            litBulbs.RemoveAt(0);
            float timer = 0;
            while (timer < interval)
            {
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }

#pragma warning disable 414
    private string TwitchHelpMessage = "Use '!{0} 01001' to put down switches 2 & 5 and put up switches 1, 3 & 4. Use '!{0} tx 12:34' to transmit at 12:34 and '!{0} hh 111:11' to clear your input at 111:11.";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        string[] CommandArray = command.Split(' ');
        if (CommandArray.First() == "tx" || CommandArray.First() == "hh")
        {
            if (CommandArray.Count() == 2)
            {
                var time = CommandArray[1].Split(':');
                int thing = 0;
                if (time.Count() == 2 && time.Where(x => int.TryParse(x, out thing)).Count() == 2 && time[1].Length == 2)
                {
                    int intTime = int.Parse(time[0]) * 60 + int.Parse(time[1]);
                    if (Mathf.FloorToInt(Bomb.GetTime()) < intTime)
                    {
                        yield return string.Format("sendtochaterror That time has already passed!", CommandArray.First());
                        yield break;
                    }
                    while (Mathf.FloorToInt(Bomb.GetTime()) != intTime)
                    {
                        yield return null;
                        if (Mathf.FloorToInt(Bomb.GetTime()) < intTime)
                        {
                            yield return string.Format("sendtochaterror Oops, I didn't press the button at the requested time! Please report this!", CommandArray.First());
                            yield break;
                        }
                    }
                    Buttons[Array.IndexOf(new[] { "tx", "hh" }, CommandArray.First())].OnInteract();
                }
                else
                {
                    yield return string.Format("sendtochaterror You didn't use the '{0}' command correctly!", CommandArray.First());
                    yield break;
                }
            }
            else
            {
                if (CommandArray.Length == 1)
                    yield return "sendtochaterror When should I press that button?";
                else
                    yield return string.Format("sendtochaterror You didn't use the '{0}' command correctly!", CommandArray.First());
                yield break;
            }
        }
        else if (command.Where(x => new[] { '0', '1' }.Contains(x)).Count() == command.Length)
        {
            if (command.Length == 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (command[i] == '1' != SwitchPositions[i])
                    {
                        Switches[i].OnInteract();
                        float timer = 0;
                        while (timer < 0.1f)
                        {
                            yield return null;
                            timer += Time.deltaTime;
                        }
                    }
                    yield return null;
                }
            }
            else
            {
                yield return string.Format("sendtochaterror That binary number is not 5 bits long!", CommandArray.First());
                yield break;
            }
        }
        else
        {
            yield return string.Format("sendtochaterror What does that command mean?", CommandArray.First());
            yield break;
        }
    }

    //IEnumerator TwitchHandleForcedSolve()
    //{
    //    for (int i = 0; i < 12; i++)
    //    {
    //        while (ButtonTexts[i].text != ButtonValues[i].ToString())
    //        {
    //            Buttons[i].OnInteract();
    //            yield return true;
    //        }
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    SubmitButton.OnInteract();
    //}
}
