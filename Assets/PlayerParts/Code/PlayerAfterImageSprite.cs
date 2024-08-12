using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{   
    [SerializeField]
    private float activeTime = 0.5f;  //Ne kadar uzun aktif kaldigi
    private float timeActivated;
    private float alpha;
    
    [SerializeField]
    private float alphaSet = 0.8f;   //Imagin En basinda oldugu alpha
    [SerializeField]
    private float alphaMultiplier = 0.99f;  //her Saniye bu degerle carparak azaltiyoruz

    public Transform player;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRenderer;
    private Color color;


    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRenderer.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void FixedUpdate() {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        spriteRenderer.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }

        if (playerSpriteRenderer.flipX == true)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

}
