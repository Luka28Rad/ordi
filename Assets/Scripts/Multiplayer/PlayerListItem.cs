using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

public class PlayerListItem : MonoBehaviour
{
    public string PlayerName;
    public int ConnectionId;
    public ulong PlayerSteamId;
    private bool AvatarReceived;
    public TMP_Text PlayerNameText;
    public Image PlayerIcon;
    public TMP_Text PlayerReadyText;
    public bool ready;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;
    private void Start() {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);

    }

    public void ChangeReadyStatus(){
        if(ready){
            PlayerReadyText.text = "Ready";
            PlayerReadyText.color = Color.green;
        } else {
            PlayerReadyText.text = "Not ready";
            PlayerReadyText.color = Color.red;
        }
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback) {
        if(callback.m_steamID.m_SteamID == PlayerSteamId) {
            LoadSteamAvatar(callback.m_iImage, PlayerIcon.GetComponent<Image>());
        } else {
            return;
        }
    }

    public void SetPlayersValues(){
        PlayerNameText.text = PlayerName;
        ChangeReadyStatus();
        if(!AvatarReceived) {GetPlayerIcon();}
    }

    void GetPlayerIcon() {
        int ImageId = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamId);
        if(ImageId == -1) {
            return;
        }
        LoadSteamAvatar(ImageId, PlayerIcon.GetComponent<Image>());
    }

    private void LoadSteamAvatar(int avatarInt, Image avatarImage)
    {
        uint width, height;
        if (SteamUtils.GetImageSize(avatarInt, out width, out height))
        {
            byte[] imageData = new byte[width * height * 4]; // RGBA format
            SteamUtils.GetImageRGBA(avatarInt, imageData, (int)(width * height * 4));

            // Create a new Texture2D to hold the image
            Texture2D avatarTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            avatarTexture.LoadRawTextureData(imageData);
            avatarTexture.Apply();

            // Convert the Texture2D to a Sprite and assign it to the avatar Image component
            Sprite avatarSprite = Sprite.Create(avatarTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            avatarImage.sprite = avatarSprite;
        }
    }
}
