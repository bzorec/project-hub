package com.um.feri.direct4me.android

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView

class PostboxAdapter(private var postboxList: List<String>) :
    RecyclerView.Adapter<PostboxAdapter.ViewHolder>() {

    // Interface for item click listener
    interface OnItemClickListener {
        fun onItemClick(postboxId: String)
        fun onItemLongClick(postboxId: String)
    }

    private var itemClickListener: OnItemClickListener? = null

    // Method to set the item click listener
    fun setOnItemClickListener(listener: OnItemClickListener) {
        itemClickListener = listener
    }

    inner class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val postboxTextView: TextView = itemView.findViewById(R.id.postboxTextView)
        private val postboxIdTextView: TextView = itemView.findViewById(R.id.postboxIdTextView)

        fun bind(postboxId: String) {
            postboxTextView.text = "Box ID: $postboxId"
            postboxIdTextView.text = "($postboxId)"

            itemView.setOnClickListener {
                itemClickListener?.onItemClick(postboxId)
            }

            itemView.setOnLongClickListener {
                itemClickListener?.onItemLongClick(postboxId)
                true // Return true to consume the long click event
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_postbox, parent, false)
        return ViewHolder(view)
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {
        val postboxId = postboxList[position]
        holder.bind(postboxId)
    }

    override fun getItemCount(): Int {
        return postboxList.size
    }

    fun setPostboxList(list: List<String>) {
        postboxList = list
    }
}
