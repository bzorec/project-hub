import android.content.Context
import androidx.lifecycle.ViewModel

class PostboxViewModel : ViewModel() {
    private val postboxList: MutableList<String> = mutableListOf()

    fun getPostboxList(): List<String> {
        return postboxList
    }

    fun addPostbox(postboxId: String) {

        postboxList.add(postboxId)
    }

    fun setPostboxList(list: List<String>) {
        postboxList.clear()
        postboxList.addAll(list)
    }

    fun savePostboxList(context: Context) {
        val sharedPreferences = context.getSharedPreferences("postbox_preferences", Context.MODE_PRIVATE)
        val editor = sharedPreferences.edit()
        editor.putStringSet("postbox_list", postboxList.toSet())
        editor.apply()
    }

    fun loadPostboxList(context: Context) {
        val sharedPreferences = context.getSharedPreferences("postbox_preferences", Context.MODE_PRIVATE)
        val savedPostboxList = sharedPreferences.getStringSet("postbox_list", emptySet())
        postboxList.clear()
        if (savedPostboxList != null) {
            postboxList.addAll(savedPostboxList)
        }
    }
}
