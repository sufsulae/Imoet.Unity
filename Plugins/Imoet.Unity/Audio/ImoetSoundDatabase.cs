using System.Collections.Generic;
using UnityEngine;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Imoet.Unity.Audio
{
    public sealed class ImoetSoundDatabase : ScriptableObject
    {
        [ExeButton("GeneratePlaylist"), SerializeField]
        private bool m_generatePlaylist;

        [Header("Music Sound Pack")]
        [SerializeField]
        private ImoetSoundPack m_musicPack;

        [Header("SFX Sound Pack")]
        [SerializeField]
        private ImoetSoundPack m_sfxPack;

        [Header("Global Sound Pack")]
        [SerializeField]
        private List<ImoetSoundPack> m_pack;

        private static ImoetSoundDatabase m_instance;
        //Singleton
        public static ImoetSoundDatabase instance
        {
            get {
                if (m_instance == null)
                    m_instance = Resources.Load<ImoetSoundDatabase>("ImoetSoundDatabase");
                return m_instance;
            }
        }

        public static List<ImoetSoundPack> packs { get { return instance.m_pack; } }
        public static ImoetSoundPack sfxPack { get { return instance.m_sfxPack; } }
        public static ImoetSoundPack musicPack { get { return instance.m_musicPack; } }

        public static ImoetSoundPack GetPack(string search)
        {
            if (search.ToLower() == "sfx")
                return sfxPack;
            if (search.ToLower() == "music")
                return musicPack;

            foreach (var pack in instance.m_pack) {
                if (pack.name == search || pack.name.Contains(search)) {
                    return pack;
                }
            }
            return null;
        }

#if UNITY_EDITOR
        public void GeneratePlaylist()
        {
            var writer = new StringBuilder();
            var templ = Encoding.ASCII.GetString(System.Convert.FromBase64String("Ly9UaGlzIHNjcmlwdCB3YXMgZ2VuZXJhdGVkIGJ5IEltb2V0LlVuaXR5LkF1ZGlvLkltb2V0U291bmQNCi8vRE8gTk9UIERJUkVDVExZIEVESVQgVEhJUyBGSUxFIElGIFlPVSBET05UIEtOT1cgV0hBVCBZT1UgRE8NCm5hbWVzcGFjZSBJbW9ldC5Vbml0eS5BdWRpb3t7DQoJcHVibGljIHN0YXRpYyBjbGFzcyBJbW9ldFNvdW5kUGxheWxpc3Qge3sNCgkJcHVibGljIHN0YXRpYyBjbGFzcyBHbG9iYWwge3sNCnswfQ0KCQl9fQ0KCQlwdWJsaWMgc3RhdGljIGNsYXNzIE11c2ljIHt7DQp7MX0NCgkJfX0NCgkJcHVibGljIHN0YXRpYyBjbGFzcyBTRlgge3sNCnsyfQ0KCQl9fQ0KCX19DQp9fQ=="));
            string globalPlaylist = "";
            string musicPlaylist = "";
            string sfxPlaylist = "";
            string res = "";

            //Global Playlist
            foreach (var sound in m_pack)
            {
                foreach (var clip in sound.audioClips)
                {
                    writer.AppendLine(string.Format("\t\t\tpublic const string {0} = \"{1}\";", clip.name.Replace(" ", "_"), clip.name));
                }
            }
            globalPlaylist = writer.ToString();
            writer.Clear();

            //Music Playlist
            foreach (var clip in m_musicPack.audioClips)
            {
                writer.AppendLine(string.Format("\t\t\tpublic const string {0} = \"{1}\";", clip.name.Replace(" ", "_"), clip.name));
            }
            musicPlaylist = writer.ToString();
            writer.Clear();

            //SFX Playlist
            foreach (var clip in m_sfxPack.audioClips)
            {
                writer.AppendLine(string.Format("\t\t\tpublic const string {0} = \"{1}\";", clip.name.Replace(" ", "_"), clip.name));
            }
            sfxPlaylist = writer.ToString();
            writer.Clear();

            //Write Down to file
            res = writer.AppendFormat(templ, globalPlaylist, musicPlaylist, sfxPlaylist).ToString();
            var path = AssetDatabase.GetAssetPath(this);
            var fInfo = new System.IO.FileInfo(path);
            System.IO.File.WriteAllText(fInfo.Directory + "/ImoetSoundPlaylist.cs", res);
            AssetDatabase.Refresh();
        }
        [MenuItem("Imoet/Sound/Create Database...")]
        private static void _createDB()
        {
            var db = CreateInstance<ImoetSoundDatabase>();

            AssetDatabase.CreateAsset(db, "Assets/Resources/ImoetSoundDatabase.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog("Database Creation", "New Sound Databases Has Been Created", "Ok");
            Selection.activeObject = db;
            Debug.Log("New Databases Has Been Created in: Assets/Resources/ImoetSoundDatabase.asset");
        }

        [MenuItem("Imoet/Sound/Select Database")]
        private static void _selectDB()
        {
            var db = AssetDatabase.LoadAssetAtPath("Assets/Resources/ImoetSoundDatabase.asset", typeof(ImoetSoundDatabase));
            if (db == null)
            {
                Debug.LogError("Failed to find ImoetSoundDatabase.asset, Please create a new one");
            }
            else
            {
                Selection.activeObject = db;
            }
        }
        [MenuItem("Imoet/Sound/Select Database", true)]
        private static bool _validateDB()
        {
            return AssetDatabase.LoadAssetAtPath("Assets/Resources/ImoetSoundDatabase.asset", typeof(ImoetSoundDatabase)) != null;
        }
#endif
    }

}