import { useState } from 'react';
import { Pressable, ScrollView, StyleSheet, Text, useWindowDimensions, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { product } from '../product';

const ink = '#10263D';
const muted = '#64717C';
const canvas = '#F7F3E8';

export default function HomeScreen() {
  const [activeTab, setActiveTab] = useState(0);
  const [confirmed, setConfirmed] = useState(false);
  const { width } = useWindowDimensions();
  const contentWidth = Math.min(width, 560);

  return (
    <SafeAreaView style={styles.safe} edges={['top', 'left', 'right']}>
      <View style={[styles.shell, { width: contentWidth }]}>
        <ScrollView contentContainerStyle={styles.content} showsVerticalScrollIndicator={false}>
          <View style={styles.topbar}>
            <Text style={styles.brand}>{product.name}</Text>
            <View style={styles.avatar} accessibilityLabel="Perfil de Gustavo"><Text style={styles.avatarText}>GG</Text></View>
          </View>

          <Text style={[styles.eyebrow, { color: product.accent }]}>{product.eyebrow}</Text>
          <Text style={styles.title}>{product.title}</Text>
          <Text style={styles.subtitle}>{product.subtitle}</Text>

          <View style={styles.metricRow}>
            <Text style={styles.metric}>{product.metric}</Text>
            <Text style={styles.metricLabel}>{product.metricLabel}</Text>
          </View>

          <View style={styles.route}>
            <View style={styles.track} />
            <View style={[styles.trackDone, { backgroundColor: product.accent, height: `${Math.max(18, product.steps.filter(step => step.done).length / product.steps.length * 100)}%` }]} />
            {product.steps.map((step, index) => (
              <View style={styles.step} key={step.title}>
                <View style={[styles.stop, { borderColor: step.done ? product.accent : '#9CA6A6', backgroundColor: step.done ? product.accent : canvas }]} />
                <View style={styles.stepCopy}>
                  <Text style={styles.stepTitle}>{step.title}</Text>
                  <Text style={styles.stepDetail}>{step.detail}</Text>
                </View>
                <Text style={styles.index}>{String(index + 1).padStart(2, '0')}</Text>
              </View>
            ))}
          </View>

          <View style={styles.panel}>
            <View style={styles.panelRow}>
              <View style={styles.panelCopy}>
                <Text style={styles.panelTitle}>{confirmed ? 'Tudo certo' : product.cardTitle}</Text>
                <Text style={styles.panelDetail}>{confirmed ? 'Atualização registrada nesta demonstração.' : product.cardDetail}</Text>
              </View>
              <View style={[styles.badge, { backgroundColor: product.accentSoft }]}><Text style={[styles.badgeText, { color: product.accent }]}>{confirmed ? 'FEITO' : product.badge}</Text></View>
            </View>
            <Pressable accessibilityRole="button" onPress={() => setConfirmed(value => !value)} style={({ pressed }) => [styles.button, pressed && styles.buttonPressed]}>
              <Text style={styles.buttonText}>{confirmed ? 'Desfazer ação' : product.action}</Text>
            </Pressable>
          </View>
          <Text style={styles.demoNote}>DADOS FICTÍCIOS · PROJETO DE PORTFÓLIO</Text>
        </ScrollView>

        <View style={styles.tabs} accessibilityRole="tablist">
          {product.tabs.map((tab, index) => (
            <Pressable key={tab} accessibilityRole="tab" accessibilityState={{ selected: activeTab === index }} onPress={() => setActiveTab(index)} style={styles.tab}>
              <View style={[styles.tabDot, activeTab === index && { backgroundColor: product.accent }]} />
              <Text style={[styles.tabText, activeTab === index && styles.tabTextActive]}>{tab}</Text>
            </Pressable>
          ))}
        </View>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safe: { flex: 1, backgroundColor: canvas, alignItems: 'center' },
  shell: { flex: 1, maxWidth: 560 },
  content: { paddingHorizontal: 22, paddingTop: 14, paddingBottom: 112 },
  topbar: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 28 },
  brand: { color: ink, fontSize: 18, fontWeight: '900', letterSpacing: -0.4 },
  avatar: { width: 40, height: 40, borderRadius: 20, backgroundColor: ink, alignItems: 'center', justifyContent: 'center' },
  avatarText: { color: '#FFF', fontWeight: '900' },
  eyebrow: { fontSize: 12, fontWeight: '900', letterSpacing: 1.4, marginBottom: 9 },
  title: { color: ink, fontSize: 34, lineHeight: 36, letterSpacing: -1.2, fontWeight: '900', maxWidth: 440 },
  subtitle: { color: muted, marginTop: 10, fontSize: 15, lineHeight: 21 },
  metricRow: { flexDirection: 'row', alignItems: 'baseline', marginTop: 26, paddingBottom: 14, borderBottomWidth: 1, borderBottomColor: '#DCD6C8' },
  metric: { color: ink, fontSize: 25, fontWeight: '900', letterSpacing: -0.6 },
  metricLabel: { color: muted, marginLeft: 8, fontSize: 13 },
  route: { position: 'relative', marginTop: 26, paddingLeft: 34 },
  track: { position: 'absolute', left: 9, top: 9, bottom: 22, width: 4, borderRadius: 4, backgroundColor: '#D9D5C9' },
  trackDone: { position: 'absolute', left: 9, top: 9, width: 4, borderRadius: 4 },
  step: { position: 'relative', flexDirection: 'row', minHeight: 70 },
  stop: { position: 'absolute', left: -32, top: 1, width: 18, height: 18, borderRadius: 9, borderWidth: 4 },
  stepCopy: { flex: 1, paddingRight: 12 },
  stepTitle: { color: ink, fontSize: 16, fontWeight: '800' },
  stepDetail: { color: muted, fontSize: 13, marginTop: 3 },
  index: { color: '#A29F95', fontSize: 12, fontWeight: '800' },
  panel: { backgroundColor: '#FFF', borderRadius: 16, padding: 18, shadowColor: ink, shadowOffset: { width: 0, height: 10 }, shadowOpacity: 0.09, shadowRadius: 18, elevation: 3 },
  panelRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' },
  panelCopy: { flex: 1, paddingRight: 12 },
  panelTitle: { color: ink, fontSize: 17, fontWeight: '900' },
  panelDetail: { color: muted, fontSize: 13, lineHeight: 19, marginTop: 4 },
  badge: { paddingHorizontal: 10, paddingVertical: 6, borderRadius: 999 },
  badgeText: { fontSize: 11, fontWeight: '900', letterSpacing: 0.5 },
  button: { marginTop: 17, minHeight: 50, borderRadius: 14, backgroundColor: ink, alignItems: 'center', justifyContent: 'center' },
  buttonPressed: { opacity: 0.82, transform: [{ scale: 0.99 }] },
  buttonText: { color: '#FFF', fontSize: 15, fontWeight: '800' },
  demoNote: { color: '#969187', textAlign: 'center', fontSize: 10, fontWeight: '800', letterSpacing: 0.8, marginTop: 20 },
  tabs: { position: 'absolute', left: 0, right: 0, bottom: 0, minHeight: 78, backgroundColor: '#FFF', borderTopWidth: 1, borderTopColor: '#DDD8CB', flexDirection: 'row', paddingBottom: 8 },
  tab: { flex: 1, minHeight: 62, alignItems: 'center', justifyContent: 'center' },
  tabDot: { width: 6, height: 6, borderRadius: 3, backgroundColor: 'transparent', marginBottom: 6 },
  tabText: { color: '#718080', fontSize: 11 },
  tabTextActive: { color: ink, fontWeight: '900' },
});
