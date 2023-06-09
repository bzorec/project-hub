package com.um.feri.direct4me.android.ui.dashboard

import com.um.feri.direct4me.android.PostboxViewModel
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.um.feri.direct4me.android.PostboxAdapter
import com.um.feri.direct4me.android.R
import com.um.feri.direct4me.android.ui.history.PostboxHistoryFragment
import makeApiRequest

class DashboardFragment : Fragment() {
    private lateinit var postboxViewModel: PostboxViewModel
    private lateinit var recyclerView: RecyclerView
    private lateinit var adapter: PostboxAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val rootView = inflater.inflate(R.layout.fragment_dashboard, container, false)
        recyclerView = rootView.findViewById(R.id.recyclerViewBoard)

        return rootView
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        postboxViewModel = ViewModelProvider(requireActivity())[PostboxViewModel::class.java]
        adapter = PostboxAdapter(postboxViewModel.getPostboxList())

        val recyclerView: RecyclerView = view.findViewById(R.id.recyclerViewBoard)
        recyclerView.layoutManager = LinearLayoutManager(requireContext())
        recyclerView.adapter = adapter

        adapter.setOnItemClickListener(object : PostboxAdapter.OnItemClickListener {
            override fun onItemClick(postboxId: String) {
                val boxId = postboxId.toInt()
                val apiKey = "9ea96945-3a37-4638-a5d4-22e89fbc998f"
                makeApiRequest(boxId, apiKey)
            }

            override fun onItemLongClick(postboxId: String) {
                val postboxHistoryFragment = PostboxHistoryFragment.newInstance(postboxId)
                requireActivity().supportFragmentManager.popBackStack()
                requireActivity().supportFragmentManager.beginTransaction()
                    .replace(R.id.nav_host_fragment_activity_main, postboxHistoryFragment)
                    .commit()
            }

        })
        postboxViewModel.observePostboxList { postboxList ->
            adapter.setPostboxList(postboxList)
            adapter.notifyDataSetChanged()
        }

    }

    override fun onResume() {
        super.onResume()
        adapter.notifyDataSetChanged()
    }
}
