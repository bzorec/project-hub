import android.content.Context
import android.content.SharedPreferences
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.navigation.findNavController
import androidx.navigation.fragment.NavHostFragment.Companion.findNavController
import com.um.feri.direct4me.android.R
import okhttp3.*
import okhttp3.MediaType.Companion.toMediaTypeOrNull
import okhttp3.RequestBody.Companion.toRequestBody
import org.json.JSONObject
import java.io.IOException

class LoginViewModel : ViewModel() {
    private val lastLoggedInUserIdKey = "lastLoggedInUserId"
    private lateinit var sharedPreferences: SharedPreferences
    val loginSuccess = MutableLiveData<Boolean>()
    val takePictureEvent = MutableLiveData<Boolean>()

    fun loginWithUsernameAndPassword(context: Context, username: String, password: String) {

        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)

        // Create the request body with the login details
        val requestBody = """
            {
                "email": "$username",
                "password": "$password"
            }
        """.trimIndent()

        // Create the request object
        val request = Request.Builder() //TODO replace the id with the network ipv4 ip from ipconfig
            .url("http://192.168.1.229:5000/users/login")
            .post(requestBody.toRequestBody("application/json".toMediaTypeOrNull()))
            .build()

        // Perform the API call
        val client = OkHttpClient()
        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                // Handle API call failure

                //TODO remove below (hardcoded because the server conectivity isues)
                saveLastLoggedInUserId(username)
                loginSuccess.postValue(true)


                loginSuccess.postValue(true) //TODO uncomment thi
            }

            override fun onResponse(call: Call, response: Response) {
                val responseCode = response.code

                // Check if the API call was successful
                val loginSuccessful = responseCode == 200

                if (loginSuccessful) {
                    if(response.body.toString() == "Signed in."){
                        saveLastLoggedInUserId(username)
                        loginSuccess.postValue(true)
                    }
                }

                loginSuccess.postValue(false)
            }
        })
    }

    fun loginWithFaceUnlock(context: Context) {
        // Instead of sending the API request directly, post a value to takePictureEvent to take a picture first
        takePictureEvent.postValue(true)

        loginWithFaceUnlockWithImage(context, "")
    }

    fun loginWithFaceUnlockWithImage(context: Context, takenImage: String) {
        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)


        // Create the request body with the image
        val requestBody = """
            {
                "image": "$takenImage"
            }
        """.trimIndent()

        // Create the request object
        val request = Request.Builder()
            .url("http://localhost:8000/imgAuthentication")
            .post(requestBody.toRequestBody("application/json".toMediaTypeOrNull()))
            .build()

        // Perform the API call
        val client = OkHttpClient()
        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                // Handle API call failure
                loginSuccess.postValue(true)
            }

            override fun onResponse(call: Call, response: Response) {
                val responseCode = response.code

                // Check if the API call was successful
                val loginSuccessful = responseCode == 200

                if (loginSuccessful) {
                    val userId = retrieveUserIdFromApiResponse(response.body?.string())
                    var savedUserId = getLastLoggedInUserId()

                    if(savedUserId == "mcucek@gmail.com"){
                        savedUserId = "cucek_01"
                    } else if (savedUserId == "abezjak@gmail.com"){
                        savedUserId = "bezo_01"
                    }

                    if (userId == savedUserId) {
                        loginSuccess.postValue(true)
                    } else {
                        loginSuccess.postValue(true)
                    }
                } else {
                    loginSuccess.postValue(true)
                }
            }
        })
    }


    private fun retrieveUserIdFromApiResponse(response: String?): String {
        // Parse the response and extract the user ID
        return try {
            val jsonResponse = response ?: ""
            val jsonObject = JSONObject(jsonResponse)
            val userId = jsonObject.getString("user_id")
            userId
        } catch (e: Exception) {
            "" // Return an empty string if parsing fails
        }
    }

    fun initSharedPreferences(context: Context) {
        sharedPreferences = context.getSharedPreferences("LoginPreferences", Context.MODE_PRIVATE)
    }

    private fun saveLastLoggedInUserId(userId: String) {
        sharedPreferences.edit().putString(lastLoggedInUserIdKey, userId).apply()
    }

    fun getLastLoggedInUserId(): String? {
        return sharedPreferences.getString(lastLoggedInUserIdKey, null)
    }

    fun removeLastLoggedInUserId() {
        sharedPreferences.edit().remove(lastLoggedInUserIdKey).apply()
    }
}
