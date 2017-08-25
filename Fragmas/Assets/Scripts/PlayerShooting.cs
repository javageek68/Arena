using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField]
    float shotCooldown = .3f;
    [SerializeField]
    int killsToWin = 5;
    [SerializeField]
    Transform firePosition;
    [SerializeField]
    ShotEffectsManager shotEffects;

    [SyncVar(hook = "OnScoreChanged")]
    int score;

    float shotRange = 50f;

    Player player;
    float ellapsedTime;
    bool canShoot;

    void Start()
    {
        player = GetComponent<Player>();
        shotEffects.Initialize();

        if (isLocalPlayer)
            canShoot = true;
    }

    [ServerCallback]
    void OnEnable()
    {
        score = 0;
    }

    void Update()
    {
        if (!canShoot)
            return;

        ellapsedTime += Time.deltaTime;

        if (CrossPlatformInputManager.GetButtonDown("Fire1") && ellapsedTime > shotCooldown)
        {
            ellapsedTime = 0f;
            CmdFireShot(firePosition.position, firePosition.forward);
        }
    }

    /// <summary>
    /// Commands will run on the server
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    [Command]
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

        //have the server determine if we hit anything
        Ray ray = new Ray(origin, direction);
        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 1f);

        bool result = Physics.Raycast(ray, out hit, shotRange);

        if (result)
        {
            PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

            if (enemy != null)
            {
                bool wasKillShot = enemy.TakeDamage();

                if (wasKillShot && ++score >= killsToWin)
                    player.Won();
            }
        }


        //have the client draw out the results
        RpcProcessShotEffects(result, hit.point);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playImpact"></param>
    /// <param name="point"></param>
    [ClientRpc]
    void RpcProcessShotEffects(bool playImpact, Vector3 point)
    {
        shotEffects.PlayShotEffects();

        if (playImpact)
            shotEffects.PlayImpactEffect(point);
    }

    void OnScoreChanged(int value)
    {
        score = value;
        if (isLocalPlayer)
            PlayerCanvas.canvas.SetKills(value);
    }

    public void FireAsBot()
    {
        CmdFireShot(firePosition.position, firePosition.forward);
    }
}