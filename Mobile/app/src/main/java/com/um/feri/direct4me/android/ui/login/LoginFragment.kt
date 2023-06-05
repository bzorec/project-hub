package com.um.feri.direct4me.android.ui.login

import LoginViewModel
import android.Manifest
import android.app.Activity
import android.content.Intent
import android.content.pm.PackageManager
import android.graphics.Bitmap
import android.os.Bundle
import android.provider.MediaStore
import android.util.Base64
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import com.um.feri.direct4me.android.MainActivity
import com.um.feri.direct4me.android.R
import com.um.feri.direct4me.android.databinding.FragmentLoginBinding
import java.io.ByteArrayOutputStream

class LoginFragment : Fragment() {
    private lateinit var loginViewModel: LoginViewModel
    private var _binding: FragmentLoginBinding? = null
    private val binding get() = _binding!!

    companion object {
        const val REQUEST_IMAGE_CAPTURE = 1
        const val REQUEST_CAMERA_PERMISSION = 1001
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        _binding = FragmentLoginBinding.inflate(inflater, container, false)
        val root: View = binding.root
        return root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        loginViewModel = ViewModelProvider(this).get(LoginViewModel::class.java)
        loginViewModel.initSharedPreferences(requireContext())

        loginViewModel.loginSuccess.observe(viewLifecycleOwner, Observer { isSuccess ->
            if (isSuccess) {
                // Close the fragment and show the nav bar and the action
                (activity as MainActivity).showBars()
                findNavController().navigate(R.id.action_loginFragment_to_navigation_home)
            }
        })

        loginViewModel.takePictureEvent.observe(viewLifecycleOwner, Observer { event ->

            if (event) {
                // Open the camera and let the user take a picture
                val takePictureIntent = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
                if (takePictureIntent.resolveActivity(requireActivity().packageManager) != null) {
                    startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE)
                }
            }
        })

        binding.loginButton.setOnClickListener {
            val username = binding.usernameEditText.text.toString()
            val password = binding.passwordEditText.text.toString()
            loginViewModel.loginWithUsernameAndPassword(requireContext(),username, password)
        }

        binding.faceUnlockButton.setOnClickListener {
            // call loginWithFaceUnlock
            if (ContextCompat.checkSelfPermission(requireContext(), Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED) {
                requestPermissions(arrayOf(Manifest.permission.CAMERA), REQUEST_CAMERA_PERMISSION)
            } else {

                val takePictureIntent = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
                if (takePictureIntent.resolveActivity(requireActivity().packageManager) != null) {
                    startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE)
                }
            }
            loginViewModel.loginWithFaceUnlock(requireContext())
        }

        binding.registerButton.setOnClickListener {
            //TODO redirect to webpage
            //findNavController().navigate(R.id.action_loginFragment_to_webViewFragment)
        }

        binding.anotherUserButton.setOnClickListener {
            // set lastLoggedInUser to null and refresh the fragment
            loginViewModel.removeLastLoggedInUserId()
            // refresh fragment:
            val navController = findNavController()
            navController.popBackStack()
            navController.navigate(R.id.loginFragment)
        }
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode == Activity.RESULT_OK) {
            val imageBitmap = data?.extras?.get("data") as Bitmap
            val encodedImage = encodeImage(imageBitmap)
            // Now call the ViewModel method again passing the taken picture
            loginViewModel.loginWithFaceUnlockWithImage(requireContext(), encodedImage)
        }
    }

    private fun encodeImage(image: Bitmap): String {
        val baos = ByteArrayOutputStream()
        image.compress(Bitmap.CompressFormat.JPEG, 100, baos)
        val imageBytes = baos.toByteArray()
        return Base64.encodeToString(imageBytes, Base64.DEFAULT)
    }

    override fun onStart() {
        super.onStart()
        observeViewModel()
    }

    private fun observeViewModel() {
        loginViewModel.getLastLoggedInUserId()?.let { userId ->
            binding.welcomeTextHello.visibility = View.VISIBLE
            binding.welcomeTextView.text = "$userId"
            binding.welcomeTextView.visibility = View.VISIBLE
            binding.faceUnlockButton.visibility = View.VISIBLE
            binding.usernameText.visibility = View.GONE
            binding.usernameEditText.visibility = View.GONE
            binding.registerButton.visibility = View.GONE
            binding.anotherUserButton.visibility = View.VISIBLE
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }

    override fun onRequestPermissionsResult(requestCode: Int, permissions: Array<String>, grantResults: IntArray) {
        when (requestCode) {
            REQUEST_CAMERA_PERMISSION -> {
                if ((grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED)) {
                    // permission was granted, yay! Do the task you need to do.
                    val takePictureIntent = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
                    if (takePictureIntent.resolveActivity(requireActivity().packageManager) != null) {
                        startActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE)
                    }
                } else {
                    // permission denied, boo! Disable the functionality that depends on this permission.
                    Toast.makeText(context, "Permission denied to use Camera", Toast.LENGTH_SHORT).show()
                }
                return
            }
            // Add other 'when' lines to check for other permissions your app might request.
            else -> {
                // Ignore all other requests.
            }
        }
    }

}
