package com.um.feri.direct4me.android

import com.um.feri.direct4me.android.ui.login.LoginViewModel
import android.content.Context
import android.content.SharedPreferences
import android.os.Bundle
import android.view.View
import androidx.activity.viewModels
import com.google.android.material.bottomnavigation.BottomNavigationView
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.FragmentManager
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import com.um.feri.direct4me.android.core.PreferencesHandler
import com.um.feri.direct4me.android.databinding.ActivityMainBinding


class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding
    private val loginViewModel: LoginViewModel by viewModels()
    lateinit var sharedPref: SharedPreferences

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        PreferencesHandler.init(applicationContext)
        
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        sharedPref = getSharedPreferences(BuildConfig.APPLICATION_ID, Context.MODE_PRIVATE)

        val navView: BottomNavigationView = binding.navView

        // Hide the nav bar initially
        navView.visibility = View.GONE
        // Hide the action bar initially
        supportActionBar?.hide()

        val navController = findNavController(R.id.nav_host_fragment_activity_main)
        val appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.navigation_dashboard, R.id.navigation_home, R.id.navigation_history
            )
        )

        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        // Set listener to handle navigation item reselection
        navView.setOnItemReselectedListener { item ->
            if (item.itemId == R.id.navigation_dashboard) {
                // Pop all fragments from the back stack to clear the stack
                supportFragmentManager.popBackStack(null, FragmentManager.POP_BACK_STACK_INCLUSIVE)
            }
        }

        // Observe loginSuccess
        loginViewModel.loginSuccess.observe(this) { loginSuccessful ->
            if (loginSuccessful) {
                // Show the nav bar and the action bar after a successful login
                navView.visibility = View.VISIBLE
                supportActionBar?.show()
            }
        }
    }

    fun showBars() {
        // Show the nav bar and the action bar
        binding.navView.visibility = View.VISIBLE
        supportActionBar?.show()
    }
}

