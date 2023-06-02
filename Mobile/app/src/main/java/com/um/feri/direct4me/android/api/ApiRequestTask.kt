import android.media.MediaPlayer
import android.os.AsyncTask
import android.util.Log
import com.google.gson.Gson
import com.um.feri.direct4me.android.model.OpenBoxRequest
import org.json.JSONObject
import java.io.File
import java.io.FileOutputStream
import java.io.IOException
import java.net.HttpURLConnection
import java.net.URL
import java.util.*

class ApiRequestTask(private val boxId: Int, private val authToken: String) : AsyncTask<Void, Void, ByteArray?>() {

    override fun doInBackground(vararg params: Void?): ByteArray? {
        val urlString = "https://api-d4me-stage.direct4.me/sandbox/v1/Access/openbox"
        val url = URL(urlString)
        val connection = url.openConnection() as HttpURLConnection

        // Set the necessary HTTP method, headers, and content type
        connection.requestMethod = "POST"
        connection.setRequestProperty("Content-Type", "application/json")
        connection.setRequestProperty("Authorization", "Bearer $authToken")
        connection.doOutput = true

        // Create the request body object
        val request = OpenBoxRequest(boxId = boxId)

        // Convert the request object to JSON
        val requestBody = Gson().toJson(request)

        // Write the request body to the connection's output stream
        val outputStream = connection.outputStream
        outputStream.write(requestBody.toByteArray())
        outputStream.flush()

        // Retrieve the response from the API
        val responseCode = connection.responseCode
        if (responseCode == HttpURLConnection.HTTP_OK) {
            val inputStream = connection.inputStream
            val responseString = inputStream.bufferedReader().use { it.readText() }

            // Parse the JSON response
            val jsonResponse = JSONObject(responseString)
            val dataString = jsonResponse.optString("data")

            // Convert the data string to a byte array
            val audioData = Base64.getDecoder().decode(dataString)

            // Return the audio data
            return audioData
        } else {
            Log.e("API Request", "Failed to retrieve audio. Response Code: $responseCode")
        }

        return null
    }

    override fun onPostExecute(result: ByteArray?) {
        super.onPostExecute(result)
        result?.let { playAudio(it) }
    }

    private fun saveAudioDataToFile(audioData: ByteArray): File? {
        try {
            val tempFile = File.createTempFile("temp_audio", ".mp3")
            val outputStream = FileOutputStream(tempFile)
            outputStream.write(audioData)
            outputStream.close()
            return tempFile
        } catch (e: IOException) {
            Log.e("Audio Playback", "Failed to save audio data to file.", e)
        }
        return null
    }

    private fun playAudio(audioData: ByteArray) {
        try {
            val audioFile = saveAudioDataToFile(audioData)
            if (audioFile != null) {
                val mediaPlayer = MediaPlayer()
                mediaPlayer.setDataSource(audioFile.path)
                mediaPlayer.prepare()
                mediaPlayer.setOnCompletionListener {
                    // Delete the file after playback ends
                    audioFile.delete()
                }
                mediaPlayer.start()
            }
        } catch (e: IOException) {
            Log.e("Audio Playback", "Failed to play audio.", e)
        }
    }
}

// Execute the API request task
fun makeApiRequest(boxId: Int, authToken: String) {
    ApiRequestTask(boxId, authToken).execute()
}
