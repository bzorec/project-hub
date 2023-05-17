package com.um.feri.direct4me.android.ui.home

import com.um.feri.direct4me.android.api.Direct4meService
import android.content.Intent
import android.media.MediaPlayer
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import com.google.zxing.integration.android.IntentIntegrator
import com.um.feri.direct4me.android.R
import com.um.feri.direct4me.android.databinding.FragmentHomeBinding
import com.um.feri.direct4me.android.model.OpenBoxRequest
import com.um.feri.direct4me.android.model.TokenResponse
import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class HomeFragment : Fragment() {

    private var _binding: FragmentHomeBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        val homeViewModel =
            ViewModelProvider(this).get(HomeViewModel::class.java)
        _binding = FragmentHomeBinding.inflate(inflater, container, false)
        val root: View = binding.root
        val textView: TextView = binding.textHome
        homeViewModel.text.observe(viewLifecycleOwner) {
            textView.text = it
        }
        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val scanButton: ImageButton = view.findViewById(R.id.scanButton)
        scanButton.setOnClickListener {
            val integrator = IntentIntegrator.forSupportFragment(this)
            integrator.setDesiredBarcodeFormats(IntentIntegrator.QR_CODE)
            integrator.setPrompt("Scan QR Code")
            integrator.setOrientationLocked(true)
            integrator.initiateScan()
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        val result = IntentIntegrator.parseActivityResult(requestCode, resultCode, data)
        if (result != null && result.contents != null) {
            val boxId = result.contents
            val tokenFormat = 0

            // Klic metode OpenBox prek API-ja
            openBox(boxId, tokenFormat)
        }
    }

    private fun openBox(boxId: String, tokenFormat: Int) {
        // Uporabita Retrofit za izvajanje HTTP klicev
        val retrofit = Retrofit.Builder()
            .baseUrl("https://api-d4me-stage.direct4.me/sandbox/v1/")
            .addConverterFactory(GsonConverterFactory.create())
            .build()

        val direct4meService = retrofit.create(Direct4meService::class.java)

        val openBoxRequest = OpenBoxRequest(
            deliveryId = 0,
            boxId = 0,
            tokenFormat = 0,
            latitude = 0,
            longitude = 0,
            qrCodeInfo = "string",
            terminalSeed = 0,
            isMultibox = true,
            doorIndex = 0,
            addAccessLog = true
        )

        val call = direct4meService.openBox(openBoxRequest)

        call.enqueue(object : Callback<TokenResponse> {
            override fun onResponse(call: Call<TokenResponse>, response: Response<TokenResponse>) {
                if (response.isSuccessful) {
                    val tokenResponse = response.body()
                    if (tokenResponse != null) {
                        val token = tokenResponse.token
                        playToken(token)
                        // Nadaljnje ukrepe za odpiranje pametnega paketnika Direct4me
                        openSmartLocker()
                    } else {
                        // Napaka: Neveljaven odgovor od strežnika
                        showToast("Napaka: Neveljaven odgovor od strežnika")
                    }
                } else {
                    // Napaka pri izvajanju klica API-ja
                    showToast("Napaka pri izvajanju klica API-ja")
                }
            }

            override fun onFailure(call: Call<TokenResponse>, t: Throwable) {
                // Napaka pri izvajanju klica API-ja
                showToast("Napaka pri izvajanju klica API-ja")
            }
        })
    }

    private fun playToken(token: String) {
        val mediaPlayer = MediaPlayer()

        try {
            mediaPlayer.setDataSource(token)
            mediaPlayer.prepare()
            mediaPlayer.setOnCompletionListener {
                // Žeton je bil predvajan, lahko nadaljujemo z odpiranjem paketnika
                openSmartLocker()
            }
            mediaPlayer.start()
        } catch (e: Exception) {
            e.printStackTrace()
        }
    }

    private fun openSmartLocker() {

    }

    private fun showToast(message: String) {
        Toast.makeText(requireContext(), message, Toast.LENGTH_SHORT).show()
    }
}

