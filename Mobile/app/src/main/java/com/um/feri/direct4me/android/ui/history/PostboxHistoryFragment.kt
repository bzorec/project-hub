package com.um.feri.direct4me.android.ui.history

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.um.feri.direct4me.android.R

class PostboxHistoryFragment : Fragment() {
    private lateinit var viewModel: PostboxHistoryViewModel
    private lateinit var recyclerView: RecyclerView

    private lateinit var adapter: PostboxHistoryAdapter

    companion object {
        private const val ARG_POSTBOX_ID = "postboxId"

        fun newInstance(postboxId: String): PostboxHistoryFragment {
            val fragment = PostboxHistoryFragment()
            val args = Bundle()
            args.putString(ARG_POSTBOX_ID, postboxId)
            fragment.arguments = args
            return fragment
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_postbox_history, container, false)

        recyclerView = view.findViewById(R.id.postboxHistoryRecyclerView)
        recyclerView.layoutManager = LinearLayoutManager(activity)
        adapter = PostboxHistoryAdapter()
        recyclerView.adapter = adapter

        viewModel = ViewModelProvider(this)[PostboxHistoryViewModel::class.java]
        viewModel.postboxHistory.observe(viewLifecycleOwner) { postboxHistory ->
            adapter.setPostboxHistory(postboxHistory)
        }

        viewModel.fetchPostboxHistory("")

        return view
    }
}

