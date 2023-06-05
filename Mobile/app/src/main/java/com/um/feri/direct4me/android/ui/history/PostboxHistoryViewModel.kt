package com.um.feri.direct4me.android.ui.history

import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.um.feri.direct4me.android.api.ApiClient
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import java.util.Date

class PostboxHistoryViewModel : ViewModel() {
    val postboxHistory: MutableLiveData<List<PostboxHistoryItem>> = MutableLiveData()

    @OptIn(DelicateCoroutinesApi::class)
    fun fetchPostboxHistory(boxGuid: String) {

        GlobalScope.launch {
            try {
                val postboxHistoryData = ApiClient.getPostboxHistory(boxGuid)
                withContext(Dispatchers.Main) {
                    postboxHistory.value = postboxHistoryData
                }
            } catch (e: Exception) {
                withContext(Dispatchers.Main) {
                    println(e.message)
                    val mockData = listOf(
                        PostboxHistoryItem(
                            Date(), "Blaz Zorec", "542", "QR", true
                        ),
                        PostboxHistoryItem(
                            Date(), "Blaz Zorec", "542", "QR", true
                        )
                    )
                    postboxHistory.value = mockData
                }
            }
        }
    }
}
