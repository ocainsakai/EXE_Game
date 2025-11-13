using UnityEngine;
using System.Collections;
using Firebase; // Thư viện Firebase
using Firebase.Auth; // Thư viện Firebase Auth

/// <summary>
/// Tự động đăng nhập ẩn danh khi khởi động game
/// </summary>
public class AuthManager : MonoBehaviour
{
    // Chúng ta lưu UserId ở đây để các script khác có thể dùng
    public static string UserId { get; private set; }
    public static bool IsLoggedIn { get; private set; }

    // Dùng
    public static AuthManager Instance { get; private set; }

    void Awake()
    {
        // Pattern Singleton để đảm bảo chỉ có 1 AuthManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ đối tượng này khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(InitializeFirebaseAndLogin());
    }

    IEnumerator InitializeFirebaseAndLogin()
    {
        Debug.Log("Đang khởi tạo Firebase...");
        var checkDependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => checkDependencyTask.IsCompleted);

        var dependencyStatus = checkDependencyTask.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("Firebase đã sẵn sàng. Kiểm tra user...");
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
            {
                Debug.Log("User chưa đăng nhập. Đang đăng nhập ẩn danh...");
                var signInTask = auth.SignInAnonymouslyAsync();
                yield return new WaitUntil(() => signInTask.IsCompleted);

                if (signInTask.IsCanceled || signInTask.IsFaulted)
                {
                    Debug.LogError("Đăng nhập ẩn danh thất bại: " + signInTask.Exception);
                    // Hiển thị lỗi cho người dùng "Không thể kết nối máy chủ"
                }
                else
                {
                    // Đăng nhập thành công!
                    FirebaseUser newUser = signInTask.Result.User;
                    UserId = newUser.UserId;
                    IsLoggedIn = true;
                    Debug.Log($"Đăng nhập ẩn danh thành công! UserId: {UserId}");
                }
            }
            else
            {
                // User đã đăng nhập từ lần trước
                UserId = auth.CurrentUser.UserId;
                IsLoggedIn = true;
                Debug.Log($"User đã đăng nhập từ trước. UserId: {UserId}");
            }
        }
        else
        {
            Debug.LogError("Không thể giải quyết các dependencies của Firebase: " + dependencyStatus);
        }
    }
}