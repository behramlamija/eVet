import 'package:flutter/material.dart';

import 'promocije_tab.dart';
import 'usluge_tab.dart';

class UslugeScreen extends StatelessWidget {
  const UslugeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 2,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(32, 24, 32, 0),
            child: Text('USLUGE I PROMOCIJE', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
          ),
          const TabBar(tabAlignment: TabAlignment.start, isScrollable: true, tabs: [Tab(text: 'Usluge'), Tab(text: 'Promocije')]),
          const Expanded(child: TabBarView(children: [UslugeTab(), PromocijeTab()])),
        ],
      ),
    );
  }
}
